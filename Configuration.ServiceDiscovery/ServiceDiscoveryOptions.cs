using System;
using System.Collections.Generic;

namespace Configuration.ServiceDiscovery
{
    public class ServiceDiscoveryOptions
    {
        private static ServiceDiscoveryOptions? _instance;

        public static ServiceDiscoveryOptions Instance => _instance ?? throw new ArgumentNullException(nameof(Instance));

        public bool AllowAllSchemes { get; private set; }
        public IReadOnlyCollection<string>? AllowedSchemes { get; private set; }

        public static ServiceDiscoveryOptions Create(
            bool allowAllSchemes = true,
            params string[] allowedSchemes)
        {
            if (_instance is null)
            {
                if (allowedSchemes is { Length: 0 })
                {
                    allowedSchemes = ["https", "http"];
                }

                _instance = new ServiceDiscoveryOptions
                {
                    AllowAllSchemes = allowAllSchemes,
                    AllowedSchemes = allowedSchemes,
                };
            }
            return _instance;
        }
    }
}
