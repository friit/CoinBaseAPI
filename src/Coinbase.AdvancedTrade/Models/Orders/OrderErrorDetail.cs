using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record OrderErrorDetail(
    [property: JsonPropertyName("message")] string Message,
    [property: JsonPropertyName("field")] string? Field);
