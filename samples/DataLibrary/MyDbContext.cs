using System.Data.Entity;
using System.Data.Entity.SqlServer;

using AspireNetFramework.Samples.DataLibrary.Models;

namespace AspireNetFramework.Samples.DataLibrary
{
    [DbConfigurationType(typeof(MicrosoftSqlDbConfiguration))]
    public class MyDbContext : DbContext
    {
        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public MyDbContext() : base()
        {
        }

        public MyDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyDbContext, Migrations.Configuration>(true));
        }
    }
}
