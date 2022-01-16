using System;
using Registrations.UserRegistration.Application.Interfaces.Configurations;

namespace Registrations.UserRegistration.Application.Core.Configurations
{
    public class EnvironmentConfig : IConfig
    {
        public string GetValue(string key)
        {
            return GetValueOrNull(key) ??
                   throw new InvalidOperationException($"Environment variable, '{key}', doesn't exist");
        }

        private static string GetValueOrNull(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
