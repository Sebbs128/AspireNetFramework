using System;

using OpenTelemetry;
using OpenTelemetry.Instrumentation.AspNet;
using OpenTelemetry.Metrics;

namespace AspireNetFramework.OpenTelemetry
{
    public partial class AspireTelemetry
    {
        private static MeterProvider _metricsProvider;

        public static void AddMetrics(
            Action<MeterProviderBuilder> configureBuilder = null,
            Action<AspNetMetricsInstrumentationOptions> configureInstrumentation = null)
        {
            var builder = Sdk.CreateMeterProviderBuilder()
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

            _metricsProvider = builder.Build();
        }
    }
}
