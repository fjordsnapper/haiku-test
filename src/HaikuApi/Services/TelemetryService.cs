using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace HaikuApi.Services;

public interface ITelemetryService
{
    void TrackFeatureFlagUsage(string featureName, bool isEnabled, long executionTimeMs, long memoryUsedBytes);
    void TrackOperationPerformance(string operationName, long executionTimeMs, bool success);
    void TrackCostImpact(string featureName, decimal estimatedCost);
}

public class TelemetryService : ITelemetryService
{
    private readonly TelemetryClient _telemetryClient;

    public TelemetryService(TelemetryClient telemetryClient)
    {
        _telemetryClient = telemetryClient;
    }

    public void TrackFeatureFlagUsage(string featureName, bool isEnabled, long executionTimeMs, long memoryUsedBytes)
    {
        var properties = new Dictionary<string, string>
        {
            { "FeatureName", featureName },
            { "IsEnabled", isEnabled.ToString() },
            { "Implementation", isEnabled ? "Optimized" : "Legacy" }
        };

        var metrics = new Dictionary<string, double>
        {
            { "ExecutionTimeMs", executionTimeMs },
            { "MemoryUsedBytes", memoryUsedBytes },
            { "MemoryUsedKB", memoryUsedBytes / 1024.0 }
        };

        _telemetryClient.TrackEvent("FeatureFlagUsage", properties, metrics);
    }

    public void TrackOperationPerformance(string operationName, long executionTimeMs, bool success)
    {
        var properties = new Dictionary<string, string>
        {
            { "OperationName", operationName },
            { "Success", success.ToString() }
        };

        var metrics = new Dictionary<string, double>
        {
            { "ExecutionTimeMs", executionTimeMs }
        };

        _telemetryClient.TrackEvent("OperationPerformance", properties, metrics);
    }

    public void TrackCostImpact(string featureName, decimal estimatedCost)
    {
        var properties = new Dictionary<string, string>
        {
            { "FeatureName", featureName },
            { "CostType", "Estimated" }
        };

        var metrics = new Dictionary<string, double>
        {
            { "EstimatedCostUSD", (double)estimatedCost }
        };

        _telemetryClient.TrackEvent("CostImpact", properties, metrics);
    }
}
