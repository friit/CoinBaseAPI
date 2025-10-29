using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record OrderErrorResponse(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("error_details")] IReadOnlyList<OrderErrorDetail> ErrorDetails);
