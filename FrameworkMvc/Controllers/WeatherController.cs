using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web.Mvc;

using FrameworkMvc.Models;


namespace FrameworkMvc.Controllers
{
    public class WeatherController : Controller
    {
        // GET: Weather
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();

            var baseUri = ResolveServiceEndpoint("ApiPath");
            var uri = new Uri(baseUri, "weatherForecast");

            var models = await client.GetFromJsonAsync<IEnumerable<WeatherForecastModel>>(uri);

            return View(models);
        }

        private Uri ResolveServiceEndpoint(string key)
        {
            var uriFromConfig = new Uri(ConfigurationManager.AppSettings[key].ToLowerInvariant());

            var serviceKey = $"{uriFromConfig.Host}__{uriFromConfig.Scheme}__0";

            return new Uri(ConfigurationManager.AppSettings[serviceKey]);
        }
    }
}