using Grafana.OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace GymApp.Configuration;
public static class TelemetryConfig
{
    public static IServiceCollection AddGymAppTelemetry(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: "gym-app",
                    serviceVersion: "1.0.0"))
            .WithTracing(tracerBuilder =>
            {
                tracerBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .UseGrafana();
            })
            .WithMetrics(meterBuilder =>
            {
                meterBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .UseGrafana();
            });

        return services;
    }
}
