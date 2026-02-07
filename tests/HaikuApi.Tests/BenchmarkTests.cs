using BenchmarkDotNet.Running;

namespace HaikuApi.Tests;

public class BenchmarkTests
{
    [Fact(Skip = "Benchmarks are run separately, not as part of unit tests")]
    public void RunPerformanceBenchmarks()
    {
        var summary = BenchmarkRunner.Run<UserServiceBenchmarks>();
        
        Assert.NotNull(summary);
    }
}
