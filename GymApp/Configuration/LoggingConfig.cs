using Serilog;
using Serilog.Sinks.OpenTelemetry;

namespace GymApp.Configuration;

public static class LoggingConfig
{
    public static IHostBuilder UseGymAppLogging(this IHostBuilder hostBuilder)
    {
        var otelEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
        var otelHeaders = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_HEADERS");
        var otelProtocol = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL");

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.OpenTelemetry(o =>
            {
                o.Endpoint = otelEndpoint;
                o.Headers = ParseHeaders(otelHeaders);
                o.Protocol = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL") switch
                {
                    "http/protobuf" => OtlpProtocol.HttpProtobuf,
                    _ => OtlpProtocol.Grpc
                };
            })
            .CreateLogger();

        return hostBuilder.UseSerilog();
    }
    /// <summary>
    /// Método auxiliar para processar a string de headers no formato "k1=v1,k2=v2"
    /// para um dicionário.
    /// </summary>
    private static IDictionary<string, string> ParseHeaders(string? headers)
    {
        var dictionary = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(headers))
        {
            foreach (var header in headers.Split(','))
            {
                var parts = header.Split('=');
                if (parts.Length == 2)
                {
                    dictionary[parts[0]] = parts[1];
                }
            }
        }
        return dictionary;
    }
}
