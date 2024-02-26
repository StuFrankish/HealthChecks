using Microsoft.Extensions.DependencyInjection;

namespace HealthChecks.Uptime;

public static class ServiceCollectionExtension
{
    public static IHealthChecksBuilder AddUptimeHealthCheck(this IHealthChecksBuilder builder)
    {
        builder.Services.AddSingleton(implementationInstance: new StartupTimeHealthCheck(DateTime.Now));
        builder.AddCheck<StartupTimeHealthCheck>(name: "startup_time");

        return builder;
    }
}