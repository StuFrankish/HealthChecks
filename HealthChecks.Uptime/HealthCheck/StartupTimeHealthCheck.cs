using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Uptime;

/// <summary>
/// Initializes a new instance of the <see cref="StartupTimeHealthCheck"/> class.
/// </summary>
/// <param name="startupTime">The startup time of the application.</param>
public sealed class StartupTimeHealthCheck(DateTime startupTime) : IHealthCheck
{
    private readonly DateTime _startupTime = startupTime;

    /// <summary>
    /// Checks the health of the application's startup time.
    /// </summary>
    /// <param name="context">The health check context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous health check operation.</returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var uptime = DateTime.Now - _startupTime;
        var data = new Dictionary<string, object>
        {
            { Constants.Language.Data_StartupTime, _startupTime.ToString(format: "o") },
            { Constants.Language.Data_Uptime, uptime.ToString() }
        };

        return Task.FromResult(HealthCheckResult.Healthy(description: Constants.Language.Data_HealthyDescription, data));
    }
}
