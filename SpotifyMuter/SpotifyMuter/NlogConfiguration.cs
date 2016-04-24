using NLog;
using NLog.Config;
using NLog.Targets;

namespace SpotifyMuter
{
    /// <summary>Configuration for Nlog</summary>
    /// <remarks>https://github.com/NLog/NLog/wiki/Configuration-API</remarks>
    public static class NlogConfiguration
    {
        public static void Configure()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            // Step 3. Set target properties 
            fileTarget.FileName = "${basedir}/SpotifyMuter.log";
            fileTarget.Layout = @"${longdate}|${level:uppercase=true}|${logger}|${message}${when:when=length('${exception}')>0:inner=${newline}${exception:format=Type, Message, StackTrace}}";
            fileTarget.DeleteOldFileOnStartup = true;

            // Step 4. Define rules

            var rule = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule);
            
            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }
    }
}