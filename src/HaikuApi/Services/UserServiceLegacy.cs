using HaikuApi.Models;

namespace HaikuApi.Services;

public class UserServiceLegacy
{
    private static readonly List<UserLegacy> Users = new();
    private static int _nextId = 1;

    public Task<IEnumerable<UserLegacy>> GetAllUsersAsync()
    {
        return Task.FromResult(Users.AsEnumerable());
    }

    public Task<UserLegacy?> GetUserByIdAsync(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<UserLegacy> CreateUserAsync(UserLegacy user)
    {
        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task<UserLegacy?> UpdateUserAsync(int id, UserLegacy user)
    {
        var existingUser = Users.FirstOrDefault(u => u.Id == id);
        if (existingUser == null)
            return Task.FromResult<UserLegacy?>(null);

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        return Task.FromResult<UserLegacy?>(existingUser);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        var user = Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
            return Task.FromResult(false);

        Users.Remove(user);
        return Task.FromResult(true);
    }
}
