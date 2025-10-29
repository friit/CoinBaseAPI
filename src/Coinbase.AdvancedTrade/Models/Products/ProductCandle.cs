using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record ProductCandle(
    [property: JsonPropertyName("start")] DateTimeOffset Start,
    [property: JsonPropertyName("high")] decimal High,
    [property: JsonPropertyName("low")] decimal Low,
    [property: JsonPropertyName("open")] decimal Open,
    [property: JsonPropertyName("close")] decimal Close,
    [property: JsonPropertyName("volume")] decimal Volume);
