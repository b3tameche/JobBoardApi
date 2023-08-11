using Microsoft.AspNetCore.Identity;
using update.Models.Domain;

namespace update.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByIdAsync(string id);
    Task<IEnumerable<IdentityError>> CreateUserAsync(User user, string password);
    Task<IEnumerable<IdentityError>> DeleteUserAsync(string id);
    Task<IEnumerable<IdentityError>> UpdateUserAsync(User user);
    Task<IEnumerable<User>> GetAllAsync();
}