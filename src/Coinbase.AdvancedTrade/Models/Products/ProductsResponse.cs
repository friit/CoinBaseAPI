using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record ProductsResponse(
    [property: JsonPropertyName("products")] IReadOnlyList<Product> Products,
    [property: JsonPropertyName("num_products")] int? NumProducts);
