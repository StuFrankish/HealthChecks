using Microsoft.Extensions.DependencyInjection;

namespace HealthChecks.Uptime;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds the Uptime health check to the <see cref="IHealthChecksBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/> to add the health check to.</param>
    /// <returns>The <see cref="IHealthChecksBuilder"/> with the Uptime health check added.</returns>
    public static IHealthChecksBuilder AddUptimeHealthCheck(this IHealthChecksBuilder builder)
    {
        builder.Services.AddSingleton(implementationInstance: new StartupTimeHealthCheck(DateTime.Now));
        builder.AddCheck<StartupTimeHealthCheck>(name: Constants.Configuration.DefaultCheckName);

        return builder;
    }
}