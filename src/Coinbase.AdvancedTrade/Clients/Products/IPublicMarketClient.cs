using Coinbase.AdvancedTrade.Models.Products;

namespace Coinbase.AdvancedTrade.Clients.Products;

public interface IPublicMarketClient
{
    Task<ProductsResponse> GetProductsAsync(string? productType = null, CancellationToken cancellationToken = default);
    Task<Product> GetProductAsync(string productId, CancellationToken cancellationToken = default);
    Task<ProductBookResponse> GetProductBookAsync(string productId, int? limit = null, CancellationToken cancellationToken = default);
    Task<ProductCandlesResponse> GetProductCandlesAsync(string productId, DateTimeOffset? start = null, DateTimeOffset? end = null, int? granularitySeconds = null, CancellationToken cancellationToken = default);
    Task<ProductStatsResponse> GetProductStatsAsync(string productId, CancellationToken cancellationToken = default);
}
