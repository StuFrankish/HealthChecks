using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HealthChecks.Uptime;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationUptimeHealthCheck(this IServiceCollection services)
    {
        services.TryAddSingleton<StartupTimeHealthCheck>();

        services.AddHealthChecks()
            .AddCheck<StartupTimeHealthCheck>(name: "startup_time");

        return services;
    }
}