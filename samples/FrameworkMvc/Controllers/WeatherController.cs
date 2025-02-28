using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web.Mvc;

using AspireNetFramework.Samples.FrameworkMvc.Models;


namespace AspireNetFramework.Samples.FrameworkMvc.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();

            var baseUri = new Uri(ConfigurationManager.AppSettings.ResolveService("ApiPath"));
            var uri = new Uri(baseUri, "weatherForecast");

            var models = await client.GetFromJsonAsync<IEnumerable<WeatherForecastModel>>(uri);

            return View(models);
        }
    }
}