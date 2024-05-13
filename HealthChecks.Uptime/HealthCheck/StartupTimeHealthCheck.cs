using HealthChecks.Uptime.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.Uptime;

/// <summary>
/// Initializes a new instance of the <see cref="StartupTimeHealthCheck"/> class.
/// </summary>
/// <param name="startupTime">The startup time of the application.</param>
public sealed class StartupTimeHealthCheck(DateTime startupTime, UptimeHealthCheckOptions? options) : IHealthCheck
{
    private readonly DateTime _startupTime = startupTime;
    private readonly UptimeHealthCheckOptions _options = options ?? new();

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
            // Formats the startup time as a string using the "o" format specifier (ISO 8601 format)
            { Constants.Language.Data_StartupTime, _startupTime.ToString(format: "o") },

            // Include the uptime as a string
            { Constants.Language.Data_Uptime, uptime.ToString() }
        };

        // Shortcut for when the degraded threshold is not set or is less than 0
        if (
            _options.DegradedThresholdInSeconds is null ||
            _options.DegradedThresholdInSeconds.HasValue && _options.DegradedThresholdInSeconds.Value < 0
        )
        {
            // Create a new HealthCheckResult object with the specified parameters
            var shortResult = new HealthCheckResult(
                status: HealthStatus.Healthy,
                description: Constants.Language.Data_HealthyDescription,
                data: data
            );

            return Task.FromResult(shortResult);
        }

        // Normal operation
        HealthStatus status;
        string description;

        if (uptime.TotalSeconds < _options.DegradedThresholdInSeconds)
        {
            status = HealthStatus.Degraded;
            description = Constants.Language.Data_DegradedDescription;
        }
        else
        {
            status = HealthStatus.Healthy;
            description = Constants.Language.Data_HealthyDescription;
        }

        // Create a new HealthCheckResult object with the specified parameters
        var result = new HealthCheckResult(
            status: status,
            description: description,
            data: data
        );

        return Task.FromResult(result);
    }
}
