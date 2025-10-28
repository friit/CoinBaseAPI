using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record CancelOrdersRequest(
    [property: JsonPropertyName("order_ids")] IReadOnlyList<string> OrderIds,
    [property: JsonPropertyName("client_order_ids")] IReadOnlyList<string>? ClientOrderIds = null);
