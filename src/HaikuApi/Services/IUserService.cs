using HaikuApi.Models;

namespace HaikuApi.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(short id);
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(short id, User user);
    Task<bool> DeleteUserAsync(short id);
}
