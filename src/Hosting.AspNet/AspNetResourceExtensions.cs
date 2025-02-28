using System.Xml.Serialization;

using Aspire.Hosting.ApplicationModel;

using AspireNetFramework.Hosting.AspNet;

namespace Aspire.Hosting;

public static class AspNetResourceExtensions
{
    public static IResourceBuilder<AspNetResource> AddAspNetProject<TProject>(this IDistributedApplicationBuilder builder, string name)
        where TProject : IProjectMetadata, new()
    {
        if (!OperatingSystem.IsWindows())
        {
            // on non-Windows, use a placeholder executable for now
            return builder.AddResource(new AspNetResource(name, $"""echo "ASP.NET (.NET Framework) projects run only on Windows. Resource '{name}' will not be run." """, "."))
                .WithAnnotation(new TProject());
        }

        var iisExpressPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express", "iisexpress.exe");

        return builder.AddResource(new AspNetResource(name, iisExpressPath, "."))
            .WithAnnotation(new TProject())
            .WithSiteDefaults<TProject>();
    }

    public static IResourceBuilder<AspNetResource> WithSiteDefaults<TProject>(this IResourceBuilder<AspNetResource> builder) where TProject : IProjectMetadata
    {
        builder.WithOtlpExporter();

        var vsDir = FindVsDirectoryPath(new(builder.ApplicationBuilder.AppHostDirectory));

        if (vsDir is null)
        {
            return builder;
        }

        var applicationHostConfigPath = FindApplicationHostConfigFilePath(new(vsDir));

        if (applicationHostConfigPath is null)
        {
            return builder;
        }

        var projectPath = builder.Resource.Annotations.OfType<TProject>().First().ProjectPath;

        var siteConfig = GetSiteConfig(applicationHostConfigPath, projectPath);

        if (siteConfig is null)
        {
            return builder;
        }

        builder.WithArgs($"/config:{applicationHostConfigPath}", $"/site:{siteConfig.Name}");

        foreach (var binding in siteConfig.Bindings)
        {
            builder.WithEndpoint(binding.Protocol, e =>
            {
                e.Port = binding.Port;
                e.UriScheme = binding.Protocol;
                e.IsProxied = false;
            },
            createIfNotExists: true);
        }

        return builder;
    }

    private static string FindVsDirectoryPath(DirectoryInfo? dir)
    {
        if (dir?.EnumerateDirectories(".vs").Count() == 1)
        {
            return Path.Combine(dir.FullName, ".vs/");
        }
        return FindVsDirectoryPath(dir?.Parent);
    }

    private static string? FindApplicationHostConfigFilePath(DirectoryInfo dir)
    {
        if (dir.EnumerateFiles("applicationhost.config").Count() == 1)
        {
            return Path.Combine(dir.FullName, "applicationhost.config");
        }
        foreach (var subdir in dir.EnumerateDirectories())
        {
            var result = FindApplicationHostConfigFilePath(subdir);

            if (result is not null)
                return result;
        }
        return null;
    }

    private static Site? GetSiteConfig(string appHostConfigPath, string projectPath)
    {
        var serializer = new XmlSerializer(typeof(ApplicationHostConfiguration));
        using var reader = new FileStream(appHostConfigPath, FileMode.Open);

        if (serializer.Deserialize(reader) is not ApplicationHostConfiguration appHostConfig)
        {
            return null;
        }

        var comparison = Environment.OSVersion.Platform == PlatformID.Win32NT
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        return appHostConfig.SystemApplicationHost.Sites
            .SingleOrDefault(s => string.Equals(
                s.Application.VirtualDirectory.PhysicalPath,
                Path.GetDirectoryName(projectPath),
                comparison));
    }
}
