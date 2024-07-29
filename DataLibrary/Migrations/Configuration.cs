using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

using DataLibrary.Models;

namespace DataLibrary.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MyDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyDbContext context)
        {
            if (!context.Cities.Any())
            {
                context.Countries.AddOrUpdate(
                    new Country
                    {
                        Name = "Australia",
                        Cities = new List<City>
                        {
                            new City { Name = "Brisbane" },
                            new City { Name = "Sydney" },
                            new City { Name = "Melbourne" },
                            new City { Name = "Canberra" },
                            new City { Name = "Adelaide" },
                            new City { Name = "Perth" },
                            new City { Name = "Hobart" },
                            new City { Name = "Darwin" },
                        }
                    },
                    new Country
                    {
                        Name = "United States",
                        Cities = new List<City>
                        {
                            new City { Name = "New York" },
                            new City { Name = "Los Angeles" },
                            new City { Name = "Chicago" },
                            new City { Name = "Seattle" },
                            new City { Name = "San Francisco" },
                            new City { Name = "Miami" },
                        }
                    });
            }

            base.Seed(context);
        }
    }
}
