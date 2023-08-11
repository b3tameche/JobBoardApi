using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using update.Models.Domain;
using update.Models.DTOs;
using update.Services;

namespace update.Controllers;

[ApiController]
[Route("/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly TokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthController(UserManager<User> userManager, TokenService tokenService, IMapper mapper) {
        this._userManager = userManager;
        this._tokenService = tokenService;
        this._mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] UserLoginDTO loginDto) {

        // retrieve user from database using specified email
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        
        // is no such user exists or provided password is incorrect...
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password)) {
            return Unauthorized();
        }
        
        // return fresh UserDTO with given email and Jwt token
        return new UserDTO {
            email = loginDto.Email,
            token = await _tokenService.GenerateToken(user)
        };
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserRegisterDTO registerDto) {

        // map UserRegisterDTO to User (firstname, lastname, company)
        var user = _mapper.Map<User>(registerDto);

        // create new user using given credentials using provided ApplicationDbContext
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        // if operation has failed...
        if (!result.Succeeded) {

            // result should contain some errors...
            foreach (var error in result.Errors) {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem();
        }

        // if operation was successful, add newly created user to "Issuer" role
        await _userManager.AddToRoleAsync(user, "Issuer");

        // created
        return StatusCode(201);
    }

}