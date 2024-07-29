using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using DataLibrary;
using DataLibrary.Models;

using FrameworkMvc.Models;

namespace FrameworkMvc.Controllers
{
    public class CitiesController : Controller
    {
        public MyDbContext Database { get; private set; }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Database = new MyDbContext(ConfigurationManager.ConnectionStrings["mydatabase"].ConnectionString);
        }

        // GET: Cities
        public ActionResult Index()
        {
            return View(Database.Cities
                .Include(city => city.Country)
                .OrderBy(city => city.Country.Name)
                .ThenBy(city => city.Name));
        }

        public ActionResult Create()
        {
            return View(new CreateCityViewModel
            {
                Countries = Database.Countries
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCityViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = Database.Countries;

                return View(model);
            }

            var city = new City
            {
                Name = model.Name,
                CountryId = model.SelectedCountryId
            };

            Database.Cities.Add(city);
            Database.SaveChanges();

            return Redirect(nameof(Index));
        }

        protected override void Dispose(bool disposing)
        {
            Database.Dispose();
            base.Dispose(disposing);
        }
    }
}