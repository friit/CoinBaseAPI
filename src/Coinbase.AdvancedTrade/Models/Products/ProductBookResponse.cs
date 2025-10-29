using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record ProductBookResponse(
    [property: JsonPropertyName("product_id")] string ProductId,
    [property: JsonPropertyName("bids")] IReadOnlyList<ProductBookEntry> Bids,
    [property: JsonPropertyName("asks")] IReadOnlyList<ProductBookEntry> Asks,
    [property: JsonPropertyName("sequence")] long Sequence);
