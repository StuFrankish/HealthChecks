using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Uptime;

public sealed class StartupTimeHealthCheck : IHealthCheck
{
    private readonly DateTime _startupTime;

    public StartupTimeHealthCheck(DateTime startupTime)
    {
        _startupTime = startupTime;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var uptime = DateTime.UtcNow - _startupTime;
        var data = new Dictionary<string, object>
        {
            { "Startup Time", _startupTime.ToString(format: "o") },
            { "Uptime", uptime.ToString() }
        };

        return Task.FromResult(HealthCheckResult.Healthy(description: "Application has been running without issues.", data));
    }
}
