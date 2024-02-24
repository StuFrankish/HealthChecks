using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Uptime;

public static class HealthCheckBuilder
{
    private const string NAME = "startup_time";

    public static IHealthChecksBuilder AddApplicationUptimeCheck(this IHealthChecksBuilder builder, 
        string? name = default,
        HealthStatus? failureStatus = default,
        IEnumerable<string>? tags = default,
        TimeSpan? timeout = default)
    {
        return builder.Add(registration: new HealthCheckRegistration(
            name ?? NAME,
            _ => new StartupTimeHealthCheck(),
            failureStatus,
            tags,
            timeout));
    }
}
