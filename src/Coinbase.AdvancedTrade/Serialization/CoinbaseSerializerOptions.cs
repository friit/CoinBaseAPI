using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Serialization;

internal static class CoinbaseSerializerOptions
{
    private static readonly JsonSerializerOptions SharedOptions = CreateDefaultOptions();

    public static JsonSerializerOptions Create()
    {
        // Return a copy to keep the shared instance immutable for callers that might mutate options.
        return new JsonSerializerOptions(SharedOptions);
    }

    private static JsonSerializerOptions CreateDefaultOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false,
            NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
        };

        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

        return options;
    }
}
