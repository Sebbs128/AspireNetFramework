using System.Xml.Linq;

using Aspire.Hosting.ApplicationModel;

using Hosting.AspNet;

namespace Aspire.Hosting;

public static class AspNetResourceExtensions
{
    public static IResourceBuilder<AspNetResource> AddAspNetProject<TProject>(this IDistributedApplicationBuilder builder, string name)
        where TProject : IProjectMetadata, new()
    {
        var iisExpressPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express", "iisexpress.exe");

        return builder.AddResource(new AspNetResource(name, iisExpressPath, "."))
            .WithAnnotation(new TProject())
            .WithSiteDefaults<TProject>();
    }

    public static IResourceBuilder<AspNetResource> WithSiteDefaults<TProject>(this IResourceBuilder<AspNetResource> builder) where TProject : IProjectMetadata
    {
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

    private static string FindVsDirectoryPath(DirectoryInfo dir)
    {
        if (dir.EnumerateDirectories(".vs").Count() == 1)
        {
            return Path.Combine(dir.FullName, ".vs/");
        }
        return FindVsDirectoryPath(dir.Parent);
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

    private static AspNetSiteConfig GetSiteConfig(string appHostConfigPath, string projectPath)
    {
        var xmlDoc = XDocument.Load(appHostConfigPath);

        var sites = xmlDoc.Element("configuration")
            .Element("system.applicationHost")
            .Element("sites");

        var site = sites.Descendants("site").FirstOrDefault(e =>
            e.Element("application")
            .Element("virtualDirectory")
            .Attribute("physicalPath").Value == Path.GetDirectoryName(projectPath));

        var virtualDirectory = new VirtualDirectory(site
            .Element("application")
            .Element("virtualDirectory")
            .Attribute("physicalPath").Value);
        var application = new Application(virtualDirectory);

        var bindings = site.Element("bindings")
            .Descendants("binding")
            .Select(e => new Binding(
                e.Attribute("protocol").Value,
                int.Parse(e.Attribute("bindingInformation").Value.Split(':')[1])))
            .ToList();

        return new AspNetSiteConfig(site.Attribute("name").Value, application, bindings);
    }
}
