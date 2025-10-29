using System.Net;
using System.Text.Json;

namespace Coinbase.AdvancedTrade.Errors;

/// <summary>
/// Represents a Coinbase API error response.
/// </summary>
public sealed class CoinbaseApiException : Exception
{
    public CoinbaseApiException(HttpStatusCode statusCode, string errorCode, string message, string? details = null, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Details = details;
    }

    public HttpStatusCode StatusCode { get; }

    public string ErrorCode { get; }

    public string? Details { get; }

    public static async Task<CoinbaseApiException> CreateAsync(HttpResponseMessage response, JsonSerializerOptions serializerOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(response);
        ArgumentNullException.ThrowIfNull(serializerOptions);

        var statusCode = response.StatusCode;
        var body = response.Content is null ? null : await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        var (code, message, details) = ParseError(body, serializerOptions);

        return new CoinbaseApiException(statusCode, code ?? "unknown_error", message ?? $"Coinbase API returned {(int)statusCode}", details ?? body);
    }

    private static (string? Code, string? Message, string? Details) ParseError(string? payload, JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            return (null, null, null);
        }

        try
        {
            var document = JsonDocument.Parse(payload);
            var root = document.RootElement;

            string? code = null;
            string? message = null;
            string? details = null;

            if (root.TryGetProperty("error", out var errorElement))
            {
                code = errorElement.GetPropertyOrDefault("code");
                message = errorElement.GetPropertyOrDefault("message");
                details = errorElement.GetPropertyOrDefault("detail");
            }
            else
            {
                code = root.GetPropertyOrDefault("code");
                message = root.GetPropertyOrDefault("message");
                details = root.GetPropertyOrDefault("detail");
            }

            return (code, message, details);
        }
        catch (JsonException)
        {
            return (null, null, payload);
        }
    }
}

internal static class JsonElementExtensions
{
    public static string? GetPropertyOrDefault(this JsonElement element, string propertyName)
    {
        if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String)
        {
            return property.GetString();
        }

        return null;
    }
}
