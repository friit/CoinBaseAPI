using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record ListOrdersResponse(
    [property: JsonPropertyName("orders")] IReadOnlyList<Order> Orders,
    [property: JsonPropertyName("has_next")] bool HasNext,
    [property: JsonPropertyName("cursor")] string? Cursor);
