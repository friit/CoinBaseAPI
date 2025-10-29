using Coinbase.AdvancedTrade.Models.Orders;

namespace Coinbase.AdvancedTrade.Clients.Orders;

/// <summary>
/// Order endpoints for Coinbase Advanced Trade.
/// </summary>
public interface IOrdersClient
{
    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">Order request payload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets order details.
    /// </summary>
    /// <param name="orderId">The order identifier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<Order> GetOrderAsync(string orderId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists historical orders with optional filtering and pagination.
    /// </summary>
    /// <param name="productId">Optional product id to filter by.</param>
    /// <param name="cursor">An opaque pagination cursor.</param>
    /// <param name="limit">The maximum number of items to return.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<ListOrdersResponse> ListOrdersAsync(string? productId = null, string? cursor = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a set of orders by identifiers or client order ids.
    /// </summary>
    /// <param name="request">Cancel request payload.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<CancelOrdersResponse> CancelOrdersAsync(CancelOrdersRequest request, CancellationToken cancellationToken = default);
}
