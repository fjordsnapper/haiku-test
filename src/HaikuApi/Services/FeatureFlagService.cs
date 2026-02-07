using HaikuApi.Configuration;
using Microsoft.Extensions.Options;

namespace HaikuApi.Services;

public interface IFeatureFlagService
{
    bool IsOptimizedDataTypesEnabled { get; }
}

public class FeatureFlagService : IFeatureFlagService
{
    private readonly FeatureFlags _featureFlags;

    public FeatureFlagService(IOptions<FeatureFlags> featureFlags)
    {
        _featureFlags = featureFlags.Value;
    }

    public bool IsOptimizedDataTypesEnabled => _featureFlags.UseOptimizedDataTypes;
}
