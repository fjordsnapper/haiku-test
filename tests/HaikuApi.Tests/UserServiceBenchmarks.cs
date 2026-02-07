using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using HaikuApi.Models;
using HaikuApi.Services;

namespace HaikuApi.Tests;

[MemoryDiagnoser]
[SimpleJob(warmupCount: 3, iterationCount: 5)]
public class UserServiceBenchmarks
{
    private UserService _optimizedService = null!;
    private UserServiceLegacy _legacyService = null!;
    private User _optimizedUser = null!;
    private UserLegacy _legacyUser = null!;

    [GlobalSetup]
    public void Setup()
    {
        _optimizedService = new UserService();
        _legacyService = new UserServiceLegacy();
        
        _optimizedUser = new User 
        { 
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john@example.com" 
        };
        
        _legacyUser = new UserLegacy 
        { 
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john@example.com" 
        };
    }

    [Benchmark(Description = "Optimized: Create User (short ID)")]
    public async Task<User> CreateUserOptimized()
    {
        return await _optimizedService.CreateUserAsync(_optimizedUser);
    }

    [Benchmark(Description = "Legacy: Create User (int ID)")]
    public async Task<UserLegacy> CreateUserLegacy()
    {
        return await _legacyService.CreateUserAsync(_legacyUser);
    }

    [Benchmark(Description = "Optimized: Get All Users (short ID)")]
    public async Task<IEnumerable<User>> GetAllUsersOptimized()
    {
        return await _optimizedService.GetAllUsersAsync();
    }

    [Benchmark(Description = "Legacy: Get All Users (int ID)")]
    public async Task<IEnumerable<UserLegacy>> GetAllUsersLegacy()
    {
        return await _legacyService.GetAllUsersAsync();
    }

    [Benchmark(Description = "Optimized: Get User By ID (short ID)")]
    public async Task<User?> GetUserByIdOptimized()
    {
        return await _optimizedService.GetUserByIdAsync(1);
    }

    [Benchmark(Description = "Legacy: Get User By ID (int ID)")]
    public async Task<UserLegacy?> GetUserByIdLegacy()
    {
        return await _legacyService.GetUserByIdAsync(1);
    }

    [Benchmark(Description = "Optimized: Update User (short ID)")]
    public async Task<User?> UpdateUserOptimized()
    {
        var user = new User 
        { 
            FirstName = "Jane", 
            LastName = "Smith", 
            Email = "jane@example.com" 
        };
        return await _optimizedService.UpdateUserAsync(1, user);
    }

    [Benchmark(Description = "Legacy: Update User (int ID)")]
    public async Task<UserLegacy?> UpdateUserLegacy()
    {
        var user = new UserLegacy 
        { 
            FirstName = "Jane", 
            LastName = "Smith", 
            Email = "jane@example.com" 
        };
        return await _legacyService.UpdateUserAsync(1, user);
    }

    [Benchmark(Description = "Optimized: Delete User (short ID)")]
    public async Task<bool> DeleteUserOptimized()
    {
        return await _optimizedService.DeleteUserAsync(1);
    }

    [Benchmark(Description = "Legacy: Delete User (int ID)")]
    public async Task<bool> DeleteUserLegacy()
    {
        return await _legacyService.DeleteUserAsync(1);
    }
}

