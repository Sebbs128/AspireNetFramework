using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Hosting.AspNet;

public class AspNetResource(string name, string path, string workingDirectory)
    : ExecutableResource(name, path, workingDirectory), IResourceWithServiceDiscovery
{
}
