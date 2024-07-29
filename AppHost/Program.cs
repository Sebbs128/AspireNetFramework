var builder = DistributedApplication.CreateBuilder(args);

var sqlDb = builder.AddSqlServer("sql")
    .WithDataVolume()
    .AddDatabase("mydatabase");

var coreApi = builder.AddProject<Projects.CoreApi>("coreApi");

var frameworkMvc = builder.AddAspNetProject<Projects.FrameworkMvc>("frameworkMvc")
    .WithReference(coreApi)
    .WithReference(sqlDb);

builder.AddProject<Projects.CoreMvc>("coreMvc")
    .WithReference(frameworkMvc)
    .WithReference(coreApi)
    .WithReference(sqlDb);

builder.Build().Run();
