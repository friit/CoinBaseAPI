using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record ProductBookEntry(
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("size")] decimal Size,
    [property: JsonPropertyName("num")] int NumberOfOrders);
