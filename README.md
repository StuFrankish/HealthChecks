[![NuGet](https://img.shields.io/nuget/vpre/HealthChecks.Uptime.svg)](https://www.nuget.org/packages/HealthChecks.Uptime)
[![.NET Build](https://github.com/StuFrankish/HealthChecks/actions/workflows/dotnet.yml/badge.svg)](https://github.com/StuFrankish/HealthChecks/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/StuFrankish/HealthChecks/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/StuFrankish/HealthChecks/actions/workflows/github-code-scanning/codeql)

# HealthChecks

This repository is dedicated to providing custom implementations of `IHealthCheck` for .NET applications, offering additional health monitoring capabilities beyond the default checks.

## Features

- **Uptime Health Check**: Monitor the uptime of your application, adding startup time and uptime information to your health reports.

## Getting Started

### Prerequisites

- .NET SDK ≥ 8.0.2
- Microsoft.Extensions.Diagnostics.HealthChecks ≥ 8.0.2
- An existing .NET application to integrate the health checks into

### Installation

You can add the Uptime Health Check to your project via NuGet:
```bash
dotnet add package HealthChecks.Uptime
```

## Configuration
### Adding to your services:
In your Startup.cs or wherever you configure services, add the uptime health check:

```c#
using HealthChecks.Uptime;

public void ConfigureServices(IServiceCollection services)
{
    // Your existing service configurations

    services.AddHealthChecks()
        .AddUptimeHealthCheck();
}
```

### NEW - HealthChecks now supports setting a degraded service threshold.
By setting a threshold, the healthcheck status will return degraded until the configured amount of time in seconds has passed.
```c#
using HealthChecks.Uptime;

public void ConfigureServices(IServiceCollection services)
{
    // Your existing service configurations

    services.AddHealthChecks()
        .AddUptimeHealthCheck((options) => {
            options.DegradedThresholdInSeconds = 120;
        });
}
```

### Setting up the endpoint (optional)
Configure the health check endpoint in your application's request pipeline:
```c#
app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});
```
> [!TIP]
> Add the `AspNetCore.HealthChecks.UI.Client` package to make use of the `UIResponseWriter` class and give the `Status` property a user-friendly translation to text.


### Running the Health Check Report Manually
You can create the Health Check report manually by getting an instance of the `HealthCheckService` and calling `CheckHealthAsyn()` as below;
```c#
// Where _provider is a local DI instance of `IServiceProvider`.
var healthCheckService = _provider.GetService<HealthCheckService>();

if (healthCheckService == null)
{
    // Optionally, return a meaningful response or handle the logic as required
    return JsonSerializer.Serialize(new { Error = "HealthCheckService is not available." });
}

var response = await healthCheckService.CheckHealthAsync();

var options = new JsonSerializerOptions
{
    WriteIndented = true, // For pretty-printing. Set to false in production for compact JSON.
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Common convention for JSON property names.
    IgnoreNullValues = true // Optional: depending on whether you want to include properties with null values.
};

string jsonReport = JsonSerializer.Serialize(response, options);
```

## Sample Output
The below JSON sample is representative of the output from both the manual report and from endpoint.
```json
{
    "status": "Healthy",
    "totalDuration": "00:00:00.0000202",
    "entries": {
        "startup_time": {
            "data": {
                "Startup Time": "2024-02-27T09:41:49.7002848+00:00",
                "Uptime": "08:23:57.5520239"
            },
            "description": "Application has been running without issues.",
            "duration": "00:00:00.0000202",
            "status": "Healthy",
            "tags": []
        }
    }
}
```
