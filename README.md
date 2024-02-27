[![.NET Build](https://github.com/StuFrankish/HealthChecks/actions/workflows/dotnet.yml/badge.svg)](https://github.com/StuFrankish/HealthChecks/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/StuFrankish/HealthChecks/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/StuFrankish/HealthChecks/actions/workflows/github-code-scanning/codeql)

# HealthChecks

This repository is dedicated to providing custom implementations of `IHealthCheck` for .NET applications, offering additional health monitoring capabilities beyond the default checks.

## Features

- **Uptime Health Check**: Monitor the uptime of your application, adding startup time and uptime information to your health reports.

## Getting Started

### Prerequisites

- .NET SDK ( >= 7.0.1 )
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

### Setting up the endpoint
Configure the health check endpoint in your application's request pipeline:
```c#
app.UseHealthChecks("/_health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});
```

### Sample Output
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
