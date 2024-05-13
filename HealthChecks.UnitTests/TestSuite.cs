using HealthChecks.Uptime;
using HealthChecks.Uptime.Options;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.UnitTests;

/// <summary>
/// Represents a test suite for the HealthChecks.
/// </summary>
public class TestSuite
{
    private readonly DateTime _fixedStartupTime = new(2023, 1, 1);
    private readonly StartupTimeHealthCheck _healthCheckWithFixedTime;
    private readonly UptimeHealthCheckOptions _healthCheckOptions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestSuite"/> class.
    /// </summary>
    public TestSuite()
    {
        _healthCheckWithFixedTime = new StartupTimeHealthCheck(_fixedStartupTime, _healthCheckOptions);
    }

    /// <summary>
    /// Tests the CheckHealthAsync method to ensure it returns a healthy status.
    /// </summary>
    /// <param name="hoursOffset">The number of hours offset from the startup time.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Theory]
    [InlineData(0)] // At startup
    [InlineData(-1)] // After 1 hour of uptime
    public async Task CheckHealthAsync_Should_ReturnHealthy(int hoursOffset)
    {
        // Arrange
        var healthCheck = hoursOffset == 0 ? _healthCheckWithFixedTime : new StartupTimeHealthCheck(DateTime.Now.AddHours(hoursOffset), _healthCheckOptions);

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Contains("Application has been running without issues.", result.Description);
    }

    /// <summary>
    /// Tests the CheckHealthAsync method to ensure it returns a healthy status with custom startup time.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Fact]
    public async Task CheckHealthAsync_Should_ReturnHealthy_WithCustomStartupTime()
    {
        // Act
        var result = await _healthCheckWithFixedTime.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        var data = result.Data;
        Assert.True(data.ContainsKey("Startup Time"));
        Assert.True(data.ContainsKey("Uptime"));
        Assert.Equal(_fixedStartupTime.ToString("o"), data["Startup Time"].ToString());
    }
    /// <summary>
    /// Tests the CheckHealthAsync method to ensure it returns a degraded status when DegradedThresholdInSeconds is set.
    /// </summary>
    /// <param name="hoursOffset">The number of hours offset from the startup time.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [Theory]
    [InlineData(0)] // At startup
    [InlineData(-1)] // After 1 hour of uptime
    public async Task CheckHealthAsync_Should_ReturnDegraded_WhenDegradedThresholdInSecondsIsSet(int hoursOffset)
    {
        // Arrange
        var degradedThresholdInSeconds = 3600; // Set the degraded threshold to 1 hour
        _healthCheckOptions.DegradedThresholdInSeconds = degradedThresholdInSeconds;

        var healthCheck = new StartupTimeHealthCheck(DateTime.Now.AddHours(hoursOffset), _healthCheckOptions);

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        if (hoursOffset == 0)
        {
            Assert.Equal(HealthStatus.Degraded, result.Status);
            Assert.Contains("Application is experiencing degraded service.", result.Description);
        }
        else
        {
            Assert.Equal(HealthStatus.Healthy, result.Status);
            Assert.Contains("Application has been running without issues.", result.Description);
        }
    }
}
