namespace HealthChecks.Uptime;

internal sealed class Constants
{
    internal sealed class Configuration
    {
        public const string DefaultCheckName = "startup_time";
    }

    internal sealed class Language
    {
        public const string Data_Uptime = "Uptime";
        public const string Data_StartupTime = "Startup Time";
        public const string Data_HealthyDescription = "Application has been running without issues.";
        public const string Data_DegradedDescription = "Application is experiencing degraded service.";
    }
}
