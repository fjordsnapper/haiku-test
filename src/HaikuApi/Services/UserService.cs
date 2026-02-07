using HaikuApi.Models;

namespace HaikuApi.Services;

public class UserService : IUserService
{
    private static readonly List<User> Users = new();
    private static short _nextId = 1;

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return Task.FromResult(Users.AsEnumerable());
    }

    public Task<User?> GetUserByIdAsync(short id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User> CreateUserAsync(User user)
    {
        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User?> UpdateUserAsync(short id, User user)
    {
        var existingUser = Users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return Task.FromResult<User?>(null);

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult<User?>(existingUser);
    }

    public Task<bool> DeleteUserAsync(short id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Task.FromResult(false);

        Users.Remove(user);
        return Task.FromResult(true);
    }
}
