using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record CancelOrdersResponse(
    [property: JsonPropertyName("results")] IReadOnlyList<CancelOrderResult> Results);

public sealed record CancelOrderResult(
    [property: JsonPropertyName("order_id")] string OrderId,
    [property: JsonPropertyName("client_order_id")] string? ClientOrderId,
    [property: JsonPropertyName("success")] bool Success,
    [property: JsonPropertyName("failure_reason")] string? FailureReason);
