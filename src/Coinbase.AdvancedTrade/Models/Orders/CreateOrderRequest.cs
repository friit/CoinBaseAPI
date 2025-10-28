using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record CreateOrderRequest(
    [property: JsonPropertyName("client_order_id")] string ClientOrderId,
    [property: JsonPropertyName("product_id")] string ProductId,
    [property: JsonPropertyName("side")] OrderSide Side,
    [property: JsonPropertyName("order_configuration")] OrderConfiguration OrderConfiguration,
    [property: JsonPropertyName("preview_only")] bool? PreviewOnly = null);
