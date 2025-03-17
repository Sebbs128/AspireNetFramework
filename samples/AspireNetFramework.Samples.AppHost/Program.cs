var builder = DistributedApplication.CreateBuilder(args);

var sqlDb = builder.AddSqlServer("sql")
    .WithDataVolume()
    .AddDatabase("mydatabase");

var coreApi = builder.AddProject<Projects.AspireNetFramework_Samples_CoreApi>("coreApi");

var frameworkMvc = builder.AddAspNetProject<Projects.AspireNetFramework_Samples_FrameworkMvc>("frameworkMvc")
    .WithReference(coreApi)
    .WithReference(sqlDb);

builder.AddProject<Projects.AspireNetFramework_Samples_CoreMvc>("coreMvc")
    .WithReference(frameworkMvc)
    .WithReference(coreApi)
    .WithReference(sqlDb);

builder.Build().Run();
