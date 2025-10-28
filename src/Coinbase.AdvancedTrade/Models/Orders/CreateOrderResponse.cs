using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record CreateOrderResponse(
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("failure_reason")] string? FailureReason,
    [property: JsonPropertyName("failure_reason_code")] string? FailureReasonCode,
    [property: JsonPropertyName("order_id")] string? OrderId,
    [property: JsonPropertyName("order_configuration")] OrderConfiguration? OrderConfiguration,
    [property: JsonPropertyName("order")] Order? Order,
    [property: JsonPropertyName("error_response")] OrderErrorResponse? ErrorResponse);
