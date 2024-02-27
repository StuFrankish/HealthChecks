using HealthChecks.Uptime;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthChecks.UnitTests;

public class TestSuite
{
    [Fact]
    public async Task CheckHealthAsync_Should_ReturnHealthy_ImmediatelyAfterStartup()
    {
        // Arrange
        var startupTime = DateTime.Now;
        var healthCheck = new StartupTimeHealthCheck(startupTime);

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Contains("Application has been running without issues.", result.Description);
    }

    [Fact]
    public async Task CheckHealthAsync_Should_ReturnHealthy_AfterSpecificUptime()
    {
        // Arrange
        var startupTime = DateTime.Now.AddHours(-1); // Simulate startup was 1 hour ago
        var healthCheck = new StartupTimeHealthCheck(startupTime);

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        Assert.Contains("Application has been running without issues.", result.Description);
    }

    [Fact]
    public async Task CheckHealthAsync_Should_ReturnHealthy_WithCustomStartupTime()
    {
        // Arrange
        var customStartupTime = new DateTime(2023, 1, 1); // Custom startup time
        var healthCheck = new StartupTimeHealthCheck(customStartupTime);

        // Act
        var result = await healthCheck.CheckHealthAsync(new HealthCheckContext());

        // Assert
        Assert.Equal(HealthStatus.Healthy, result.Status);
        var data = result.Data;
        Assert.True(data.ContainsKey("Startup Time"));
        Assert.True(data.ContainsKey("Uptime"));
        Assert.Equal(customStartupTime.ToString("o"), data["Startup Time"].ToString());
    }

}