using System.Text.Json.Serialization;
using Coinbase.AdvancedTrade.Models.Common;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record Order(
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("client_order_id")] string ClientOrderId,
    [property: JsonPropertyName("product_id")] string ProductId,
    [property: JsonPropertyName("side")] OrderSide Side,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("time_in_force")] string? TimeInForce,
    [property: JsonPropertyName("order_configuration")] OrderConfiguration OrderConfiguration,
    [property: JsonPropertyName("filled_size")] decimal FilledSize,
    [property: JsonPropertyName("average_filled_price")] decimal? AverageFilledPrice,
    [property: JsonPropertyName("total_fees")] MoneyAmount? TotalFees,
    [property: JsonPropertyName("created_time")] DateTimeOffset CreatedTime,
    [property: JsonPropertyName("last_fill_time")] DateTimeOffset? LastFillTime,
    [property: JsonPropertyName("expiry_time")] DateTimeOffset? ExpiryTime,
    [property: JsonPropertyName("cancel_message")] string? CancelMessage);
