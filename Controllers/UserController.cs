using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using update.Models.Domain;
using update.Models.DTOs;
using update.Repositories.Interfaces;

[ApiController]
[Route("/api/[controller]")]
public class UserController : ControllerBase
{

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper) {
        this._userRepository = userRepository;
        this._mapper = mapper;
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [HttpGet]
    public async Task<IEnumerable<User>> GetAll() {
        return await _userRepository.GetAllAsync();
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserGetByIdDTO>> GetById([FromRoute] string id) {
        var isAdmin = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value.Equals("Admin");

        if (isAdmin == false) {
            var requesterId = HttpContext.User.FindFirst("Id")?.Value;

            if (requesterId != id) {
                return BadRequest("Provided id does not belong to one requesting the deletion.");
            }
        }

        var user = await _userRepository.GetUserByIdAsync(id);

        if (user == null) {
            return BadRequest("User with given id does not exist.");
        }

        var dto = _mapper.Map<UserGetByIdDTO>(user);

        return Ok(dto);
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> Delete([FromRoute] string id) {
        var isAdmin = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value.Equals("Admin");

        if (isAdmin == false) {
            var requesterId = HttpContext.User.FindFirst("Id")?.Value;

            if (requesterId != id) {
                return BadRequest("Provided id does not belong to one requesting the deletion.");
            }
        }

        var possibleErrors = await _userRepository.DeleteUserAsync(id);

        return ErrorOrDefault(possibleErrors, 200, 500);
    }

    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Issuer")]
    [HttpPut("Update")]
    public async Task<ActionResult> Update([FromBody] UserUpdateDTO userUpdateDTO) {
        var requesterId = HttpContext.User.FindFirst("Id")?.Value;

        if (requesterId != userUpdateDTO.Id) {
            return BadRequest("Provided user id does not belong to one requesting the update.");
        }
        
        User user = await _userRepository.GetUserByIdAsync(userUpdateDTO.Id);

        user.FirstName = userUpdateDTO.FirstName;
        user.LastName = userUpdateDTO.LastName;
        user.Email = userUpdateDTO.Email;
        user.Company = userUpdateDTO.Company;
        user.UserName = userUpdateDTO.Email;

        var possibleErrors = await _userRepository.UpdateUserAsync(user);

        return ErrorOrDefault(possibleErrors, 200, 500);
    }

    private ActionResult ErrorOrDefault(IEnumerable<IdentityError> possibleErrors, int success, int failure) {
        foreach (var error in possibleErrors) {
            this.ModelState.AddModelError(error.Code, error.Description);
        }

        if (this.ModelState.ErrorCount > 0) {
            return StatusCode(failure);
        }

        return StatusCode(success);
    }

}