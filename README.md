[![.NET Build](https://github.com/StuFrankish/HealthChecks/actions/workflows/dotnet.yml/badge.svg)](https://github.com/StuFrankish/HealthChecks/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/StuFrankish/HealthChecks/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/StuFrankish/HealthChecks/actions/workflows/github-code-scanning/codeql)

# HealthChecks
A small repo to document and hold the odd custom IHealthCheck implementation as needed.

Currently available healthchecks:
- Healthchecks.Uptime

## HealthChecks.Uptime
The uptime health check extends the existing `IServiceCollection.AddHealthChecks()` functionality by adding a startup time and uptime counter to the list of entries in the health report.

### Installation
Either through your editors Package Manager or your preferred method, following https://www.nuget.org/packages/HealthChecks.Uptime

### Usage (using a `Startup.cs` sample)
```c#
using HealthChecks.Uptime;

public void ConfigureServices(IServiceCollection services)
{
   ... Rest of your code

    // Add Healthchecks
    services.AddHealthChecks()
        .AddUptimeHealthCheck();
}
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
Note, the above sample makes use of `AspNetCore.HealthChecks.UI.Client` and the following `.UseHealthChecks()` configuration.
```c#
app.UseHealthChecks(path: "/_health", options: new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    AllowCachingResponses = false
});
```
