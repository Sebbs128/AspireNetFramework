using AspireNetFramework.OpenTelemetry;

using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace AspireNetFramework.Samples.FrameworkMvc
{
    public class OpenTelemetryConfig
    {

        public static void RegisterTelemetry()
        {
            AspireTelemetry.AddMetrics(builder =>
            {
                builder.AddConsoleExporter();
                //builder.AddHttpClientInstrumentation();
                //builder.AddRuntimeInstrumentation();
            });

            AspireTelemetry.AddTracing(builder =>
            {
                builder.AddConsoleExporter();
                //builder.AddHttpClientInstrumentation();
                //builder.AddRuntimeInstrumentation();
            });
        }
    }
}