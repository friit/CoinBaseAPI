using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record ProductStatsResponse(
    [property: JsonPropertyName("product_id")] string ProductId,
    [property: JsonPropertyName("price")] decimal Price,
    [property: JsonPropertyName("price_percentage_change_24h")] decimal PricePercentageChange24H,
    [property: JsonPropertyName("volume_24h")] decimal Volume24H,
    [property: JsonPropertyName("low_24h")] decimal Low24H,
    [property: JsonPropertyName("high_24h")] decimal High24H,
    [property: JsonPropertyName("low_52w")] decimal? Low52W,
    [property: JsonPropertyName("high_52w")] decimal? High52W);
