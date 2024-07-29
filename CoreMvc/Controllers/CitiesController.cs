using System.Data.Entity;

using DataLibrary;

using Microsoft.AspNetCore.Mvc;

namespace CoreMvc.Controllers
{
    public class CitiesController(MyDbContext dbContext) : Controller
    {
        private readonly MyDbContext _dbContext = dbContext;

        public IActionResult Index()
        {
            return View(_dbContext.Cities
                .Include(city => city.Country)
                .OrderBy(city => city.Country.Name)
                .ThenBy(city => city.Name));
        }
    }
}
