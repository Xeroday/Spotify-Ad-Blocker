/* SpotifyMuter - A simple Spotify Ad Muter for Windows
 * Copyright(C) 2016 Maschmi
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see<http://www.gnu.org/licenses/>.*/

using NLog;
using NLog.Config;
using NLog.Targets;

namespace SpotifyMuter
{
    /// <summary>Configuration for Nlog</summary>
    /// <remarks>https://github.com/NLog/NLog/wiki/Configuration-API</remarks>
    internal static class NlogConfiguration
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