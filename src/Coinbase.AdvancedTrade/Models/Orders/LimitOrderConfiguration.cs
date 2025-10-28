using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record LimitOrderConfiguration(
    [property: JsonPropertyName("base_size")] decimal BaseSize,
    [property: JsonPropertyName("limit_price")] decimal LimitPrice,
    [property: JsonPropertyName("post_only")] bool? PostOnly);
