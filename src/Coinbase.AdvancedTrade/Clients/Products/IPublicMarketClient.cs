using Coinbase.AdvancedTrade.Models.Products;

namespace Coinbase.AdvancedTrade.Clients.Products;

/// <summary>
/// Public market data endpoints for Coinbase Advanced Trade.
/// </summary>
public interface IPublicMarketClient
{
    /// <summary>
    /// Gets the list of products (markets).
    /// </summary>
    /// <param name="productType">Optional product type filter (e.g., SPOT).
    /// </param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<ProductsResponse> GetProductsAsync(string? productType = null, CancellationToken cancellationToken = default);

    /// <summary>Gets details for a single product.</summary>
    /// <param name="productId">The product identifier (e.g., BTC-USD).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<Product> GetProductAsync(string productId, CancellationToken cancellationToken = default);

    /// <summary>Gets the order book for a product.</summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="limit">Optional depth limit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<ProductBookResponse> GetProductBookAsync(string productId, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>Gets historical candles for a product.</summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="start">Optional start time.</param>
    /// <param name="end">Optional end time.</param>
    /// <param name="granularitySeconds">Optional granularity in seconds.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<ProductCandlesResponse> GetProductCandlesAsync(string productId, DateTimeOffset? start = null, DateTimeOffset? end = null, int? granularitySeconds = null, CancellationToken cancellationToken = default);

    /// <summary>Gets 24-hour stats for a product.</summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<ProductStatsResponse> GetProductStatsAsync(string productId, CancellationToken cancellationToken = default);
}
