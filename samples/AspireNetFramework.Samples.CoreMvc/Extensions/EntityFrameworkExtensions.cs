using System.Data.Common;
using System.Data.Entity.SqlServer;

using AspireNetFramework.Samples.DataLibrary;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AspireNetFramework.Samples.CoreMvc.Extensions;

public static class EntityFrameworkExtensions
{
    public static IHostApplicationBuilder AddEntityFramework6(this IHostApplicationBuilder builder, string connectionName)
    {
        DbProviderFactories.RegisterFactory(MicrosoftSqlProviderServices.ProviderInvariantName, Microsoft.Data.SqlClient.SqlClientFactory.Instance);

        if (builder.Configuration.GetConnectionString(connectionName) is string connectionString)
        {

            builder.Services.TryAddScoped(_ => new MyDbContext(connectionString));
        }

        return builder;
    }
}
