namespace Hosting.AspNet;

internal class AspNetSiteConfig(string name, Application application, IReadOnlyCollection<Binding> bindings)
{
    public string Name { get; } = name;
    public Application Application { get; } = application;
    public IReadOnlyCollection<Binding> Bindings { get; } = bindings;
}

internal class Application(VirtualDirectory virtualDirectory)
{
    public VirtualDirectory VirtualDirectory { get; } = virtualDirectory;
}

internal class VirtualDirectory(string physicalPath)
{
    public string PhysicalPath { get; } = physicalPath;
}

internal class Binding(string protocol, int port)
{
    public string Protocol { get; } = protocol;
    public int Port { get; } = port;
}
