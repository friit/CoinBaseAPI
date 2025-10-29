using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record MarketOrderConfiguration(
    [property: JsonPropertyName("quote_size")] decimal? QuoteSize,
    [property: JsonPropertyName("base_size")] decimal? BaseSize);
