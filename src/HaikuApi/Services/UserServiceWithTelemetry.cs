using HaikuApi.Models;
using System.Diagnostics;

namespace HaikuApi.Services;

public class UserServiceWithTelemetry : IUserService
{
    private readonly IUserService _userService;
    private readonly IFeatureFlagService _featureFlagService;
    private readonly ITelemetryService _telemetryService;

    public UserServiceWithTelemetry(
        IUserService userService,
        IFeatureFlagService featureFlagService,
        ITelemetryService telemetryService)
    {
        _userService = userService;
        _featureFlagService = featureFlagService;
        _telemetryService = telemetryService;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await _userService.GetAllUsersAsync();
            sw.Stop();
            
            _telemetryService.TrackFeatureFlagUsage(
                "GetAllUsers",
                _featureFlagService.IsOptimizedDataTypesEnabled,
                sw.ElapsedMilliseconds,
                GC.GetTotalMemory(false)
            );
            
            return result;
        }
        catch
        {
            sw.Stop();
            _telemetryService.TrackOperationPerformance("GetAllUsers", sw.ElapsedMilliseconds, false);
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(short id)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await _userService.GetUserByIdAsync(id);
            sw.Stop();
            
            _telemetryService.TrackFeatureFlagUsage(
                "GetUserById",
                _featureFlagService.IsOptimizedDataTypesEnabled,
                sw.ElapsedMilliseconds,
                GC.GetTotalMemory(false)
            );
            
            return result;
        }
        catch
        {
            sw.Stop();
            _telemetryService.TrackOperationPerformance("GetUserById", sw.ElapsedMilliseconds, false);
            throw;
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await _userService.CreateUserAsync(user);
            sw.Stop();
            
            _telemetryService.TrackFeatureFlagUsage(
                "CreateUser",
                _featureFlagService.IsOptimizedDataTypesEnabled,
                sw.ElapsedMilliseconds,
                GC.GetTotalMemory(false)
            );
            
            return result;
        }
        catch
        {
            sw.Stop();
            _telemetryService.TrackOperationPerformance("CreateUser", sw.ElapsedMilliseconds, false);
            throw;
        }
    }

    public async Task<User?> UpdateUserAsync(short id, User user)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await _userService.UpdateUserAsync(id, user);
            sw.Stop();
            
            _telemetryService.TrackFeatureFlagUsage(
                "UpdateUser",
                _featureFlagService.IsOptimizedDataTypesEnabled,
                sw.ElapsedMilliseconds,
                GC.GetTotalMemory(false)
            );
            
            return result;
        }
        catch
        {
            sw.Stop();
            _telemetryService.TrackOperationPerformance("UpdateUser", sw.ElapsedMilliseconds, false);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(short id)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            sw.Stop();
            
            _telemetryService.TrackFeatureFlagUsage(
                "DeleteUser",
                _featureFlagService.IsOptimizedDataTypesEnabled,
                sw.ElapsedMilliseconds,
                GC.GetTotalMemory(false)
            );
            
            return result;
        }
        catch
        {
            sw.Stop();
            _telemetryService.TrackOperationPerformance("DeleteUser", sw.ElapsedMilliseconds, false);
            throw;
        }
    }
}
