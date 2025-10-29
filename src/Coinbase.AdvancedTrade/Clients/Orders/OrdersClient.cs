using System.Net.Http;
using Coinbase.AdvancedTrade.Http;
using Coinbase.AdvancedTrade.Models.Orders;
using Microsoft.Extensions.Logging;

namespace Coinbase.AdvancedTrade.Clients.Orders;

internal sealed class OrdersClient : CoinbaseClientBase, IOrdersClient
{
    public OrdersClient(ICoinbaseHttpClient httpClient, ILogger<OrdersClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var response = await SendAsync<CreateOrderRequest, CreateOrderResponse>(HttpMethod.Post, "orders", request, cancellationToken).ConfigureAwait(false);
        return response ?? throw new InvalidOperationException("Coinbase API returned an empty create order response.");
    }

    public async Task<Order> GetOrderAsync(string orderId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(orderId);
        var order = await SendAsync<Order>(HttpMethod.Get, $"orders/historical/{orderId}", cancellationToken).ConfigureAwait(false);
        return order ?? throw new InvalidOperationException($"Coinbase API returned an empty order payload for '{orderId}'.");
    }

    public async Task<ListOrdersResponse> ListOrdersAsync(string? productId = null, string? cursor = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(productId))
        {
            query["product_id"] = productId;
        }

        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query["cursor"] = cursor;
        }

        if (limit.HasValue)
        {
            query["limit"] = limit.Value.ToString();
        }

        var response = query.Count > 0
            ? await SendWithQueryAsync<ListOrdersResponse>(HttpMethod.Get, "orders/historical/batch", query, cancellationToken).ConfigureAwait(false)
            : await SendAsync<ListOrdersResponse>(HttpMethod.Get, "orders/historical/batch", cancellationToken).ConfigureAwait(false);

        return response ?? throw new InvalidOperationException("Coinbase API returned an empty orders collection.");
    }

    public async Task<CancelOrdersResponse> CancelOrdersAsync(CancelOrdersRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if ((request.OrderIds == null || request.OrderIds.Count == 0) && (request.ClientOrderIds == null || request.ClientOrderIds.Count == 0))
        {
            throw new ArgumentException("At least one order id or client order id must be provided.", nameof(request));
        }

        var response = await SendAsync<CancelOrdersRequest, CancelOrdersResponse>(HttpMethod.Post, "orders/batch_cancel", request, cancellationToken).ConfigureAwait(false);
        return response ?? throw new InvalidOperationException("Coinbase API returned an empty cancel order response.");
    }
}
