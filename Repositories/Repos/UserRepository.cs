using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using update.Models.Domain;
using update.Repositories.Interfaces;

namespace update.Repositories.Repos;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public async Task<IEnumerable<IdentityError>> CreateUserAsync(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        return result.Errors;
    }

    public async Task<IEnumerable<IdentityError>> DeleteUserAsync(string id)
    {

        var user = await _userManager.FindByIdAsync(id);

        if (user == null) {
            throw new ArgumentException("User with given id was not found.");
        }

        var result = await _userManager.DeleteAsync(user);

        return result.Errors;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var result = await _userManager.Users.ToListAsync();

        return result;
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        var result = await _userManager.FindByIdAsync(id);

        return result;
    }

    public async Task<IEnumerable<IdentityError>> UpdateUserAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);

        return result.Errors;
    }
}