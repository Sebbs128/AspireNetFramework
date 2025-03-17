using System;

using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNet;
using OpenTelemetry.Trace;

namespace AspireNetFramework.OpenTelemetry
{
    public partial class AspireTelemetry
    {
        private static TracerProvider _tracerProvider;

        public static void AddTracing(
            Action<TracerProviderBuilder> configureBuilder = null,
            Action<AspNetTraceInstrumentationOptions> configureInstrumentation = null)
        {
            var builder = Sdk.CreateTracerProviderBuilder()
                .AddOtlpExporter(ConfigureOtlpExporter)
                .ConfigureResource(ConfigureFromEnvironmentVariables);

            if (configureInstrumentation is null)
            {
                builder.AddAspNetInstrumentation();
            }
            else
            {
                builder.AddAspNetInstrumentation(configureInstrumentation);
            }

            configureBuilder?.Invoke(builder);

            _tracerProvider = builder.Build();
        }
    }
}
