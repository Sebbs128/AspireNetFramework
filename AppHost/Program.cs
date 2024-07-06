var builder = DistributedApplication.CreateBuilder(args);

var coreApi = builder.AddProject<Projects.CoreApi>("coreApi");

var frameworkMvc = builder.AddAspNetProject<Projects.FrameworkMvc>("frameworkMvc")
    .WithReference(coreApi);

builder.AddProject<Projects.CoreMvc>("coreMvc")
    .WithReference(frameworkMvc)
    .WithReference(coreApi);


builder.Build().Run();
