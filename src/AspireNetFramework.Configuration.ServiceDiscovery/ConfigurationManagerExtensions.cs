using System.Collections.Specialized;
using System.Linq;

using AspireNetFramework.Configuration.ServiceDiscovery;

namespace System.Configuration;

public static class ConfigurationManagerExtensions
{
    private static ServiceDiscoveryOptions _options => ServiceDiscoveryOptions.Instance;

    public static string? ResolveService(this NameValueCollection appSettings, string key)
    {
        var value = appSettings[key];

        if (value is null)
        {
            return null;
        }

        if (!Uri.TryCreate(value.ToLowerInvariant(), UriKind.Absolute, out var uri))
        {
            return value;
        }

        string scheme = uri.Scheme;

        if (uri.Scheme.IndexOf('+') > 0)
        {
            scheme = uri.Scheme.Split('+')[0];
            if (!_options.AllowAllSchemes
                && !_options.AllowedSchemes.Contains(scheme, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"The scheme '{scheme}' is not allowed.");
            }
        }

        var serviceKey = $"services__{uri.Host}__{scheme}__0";

        return appSettings[serviceKey] ?? value;
    }
}
