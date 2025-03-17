using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AspireNetFramework.OpenTelemetry;

namespace AspireNetFramework.Samples.FrameworkMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            for (int i = 0; i < 10 || !Debugger.IsAttached; i++)
            {
                Task.Delay(1000).Wait();
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            OpenTelemetryConfig.RegisterTelemetry();

            SystemWebAdapterConfiguration.AddSystemWebAdapters(this)
                .AddRemoteAppServer(options =>
                {
                    options.ApiKey = ConfigurationManager.AppSettings["RemoteAppApiKey"];
                });
        }

        protected void Application_End()
        {
            AspireTelemetry.Shutdown();
        }
    }
}
