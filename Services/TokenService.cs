using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using update.Models.Domain;

namespace update.Services;

public class TokenService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public TokenService(UserManager<User> userManager, IConfiguration config)
    {
        this._userManager = userManager;
        this._config = config;
    }

    public async Task<string> GenerateToken(User user) {

        // identity should have custom claims...
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("Fullname", user.FirstName + user.LastName),
            new Claim("Id", user.Id)
        };

        // identity should have role claims...
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // obtain security key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));

        // initialize credentials
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        // create token options
        var tokenOptions = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );

        // return generated serialized token
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
}