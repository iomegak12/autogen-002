namespace TrainingAgent;

using System;
using Microsoft.Extensions.Configuration;

public static class ConfigurationUtils
{
    public static IConfigurationRoot _configuration;
    public static IConfigurationRoot Configuration
    {
        get
        {
            if (_configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                _configuration = builder.Build();
            }

            return _configuration;
        }
    }

    public static string GetConfigurationValue(string key)
    {
        return Configuration[key] ?? string.Empty;
    }
}