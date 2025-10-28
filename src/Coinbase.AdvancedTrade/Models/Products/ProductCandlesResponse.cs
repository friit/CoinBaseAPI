using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record ProductCandlesResponse(
    [property: JsonPropertyName("candles")] IReadOnlyList<ProductCandle> Candles);
