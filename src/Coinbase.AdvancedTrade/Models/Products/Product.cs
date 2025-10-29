using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Products;

public sealed record Product(
    [property: JsonPropertyName("product_id")] string ProductId,
    [property: JsonPropertyName("base_currency")] string BaseCurrency,
    [property: JsonPropertyName("quote_currency")] string QuoteCurrency,
    [property: JsonPropertyName("display_name")] string DisplayName,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("status_message")] string? StatusMessage,
    [property: JsonPropertyName("price_increment")] decimal PriceIncrement,
    [property: JsonPropertyName("base_increment")] decimal BaseIncrement,
    [property: JsonPropertyName("quote_increment")] decimal QuoteIncrement,
    [property: JsonPropertyName("min_market_funds")] decimal? MinMarketFunds,
    [property: JsonPropertyName("list_status")] string? ListStatus);
