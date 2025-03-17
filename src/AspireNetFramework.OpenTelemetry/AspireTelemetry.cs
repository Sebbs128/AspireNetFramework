using System.Net.Http;

using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AspireNetFramework.OpenTelemetry
{
    public partial class AspireTelemetry
    {
        public static void Shutdown()
        {
            _tracerProvider?.Shutdown();
            _tracerProvider?.Dispose();

            _metricsProvider?.Shutdown();
            _metricsProvider?.Dispose();
        }

        private static void ConfigureFromEnvironmentVariables(ResourceBuilder builder)
        {
            builder.AddEnvironmentVariableDetector();
        }

        private static void ConfigureOtlpExporter(OtlpExporterOptions options)
        {
            // only way to get HTTP/2 to work in .NET Framework is via an override of the WinHttpHandler
            options.HttpClientFactory = CreateHttpClient;
        }

        private static HttpClient CreateHttpClient()
        {
            return new HttpClient(new Http2Handler());
        }
    }
}
