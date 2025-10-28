using Coinbase.AdvancedTrade.Models.Orders;

namespace Coinbase.AdvancedTrade.Clients.Orders;

public interface IOrdersClient
{
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<Order> GetOrderAsync(string orderId, CancellationToken cancellationToken = default);
    Task<ListOrdersResponse> ListOrdersAsync(string? productId = null, string? cursor = null, int? limit = null, CancellationToken cancellationToken = default);
    Task<CancelOrdersResponse> CancelOrdersAsync(CancelOrdersRequest request, CancellationToken cancellationToken = default);
}
