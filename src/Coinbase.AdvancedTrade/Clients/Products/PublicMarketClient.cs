using System.Net.Http;
using Coinbase.AdvancedTrade.Http;
using Coinbase.AdvancedTrade.Models.Products;
using Microsoft.Extensions.Logging;

namespace Coinbase.AdvancedTrade.Clients.Products;

internal sealed class PublicMarketClient : CoinbaseClientBase, IPublicMarketClient
{
    public PublicMarketClient(ICoinbaseHttpClient httpClient, ILogger<PublicMarketClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<ProductsResponse> GetProductsAsync(string? productType = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(productType))
        {
            query["product_type"] = productType;
        }

        var response = query.Count > 0
            ? await SendWithQueryAsync<ProductsResponse>(HttpMethod.Get, "products", query, cancellationToken).ConfigureAwait(false)
            : await SendAsync<ProductsResponse>(HttpMethod.Get, "products", cancellationToken).ConfigureAwait(false);

        return response ?? throw new InvalidOperationException("Coinbase API returned an empty products response.");
    }

    public async Task<Product> GetProductAsync(string productId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);
        var product = await SendAsync<Product>(HttpMethod.Get, $"products/{productId}", cancellationToken).ConfigureAwait(false);
        return product ?? throw new InvalidOperationException($"Coinbase API returned an empty product payload for '{productId}'.");
    }

    public async Task<ProductBookResponse> GetProductBookAsync(string productId, int? limit = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);

        var query = new Dictionary<string, string?>();
        if (limit.HasValue)
        {
            query["limit"] = limit.Value.ToString();
        }

        var response = query.Count > 0
            ? await SendWithQueryAsync<ProductBookResponse>(HttpMethod.Get, $"products/{productId}/book", query, cancellationToken).ConfigureAwait(false)
            : await SendAsync<ProductBookResponse>(HttpMethod.Get, $"products/{productId}/book", cancellationToken).ConfigureAwait(false);

        return response ?? throw new InvalidOperationException($"Coinbase API returned an empty order book payload for '{productId}'.");
    }

    public async Task<ProductCandlesResponse> GetProductCandlesAsync(string productId, DateTimeOffset? start = null, DateTimeOffset? end = null, int? granularitySeconds = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);

        var query = new Dictionary<string, string?>();
        if (start.HasValue)
        {
            query["start"] = start.Value.ToUnixTimeSeconds().ToString();
        }

        if (end.HasValue)
        {
            query["end"] = end.Value.ToUnixTimeSeconds().ToString();
        }

        if (granularitySeconds.HasValue)
        {
            query["granularity"] = granularitySeconds.Value.ToString();
        }

        var response = query.Count > 0
            ? await SendWithQueryAsync<ProductCandlesResponse>(HttpMethod.Get, $"products/{productId}/candles", query, cancellationToken).ConfigureAwait(false)
            : await SendAsync<ProductCandlesResponse>(HttpMethod.Get, $"products/{productId}/candles", cancellationToken).ConfigureAwait(false);

        return response ?? throw new InvalidOperationException($"Coinbase API returned an empty candles payload for '{productId}'.");
    }

    public async Task<ProductStatsResponse> GetProductStatsAsync(string productId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(productId);
        var stats = await SendAsync<ProductStatsResponse>(HttpMethod.Get, $"products/{productId}/stats", cancellationToken).ConfigureAwait(false);
        return stats ?? throw new InvalidOperationException($"Coinbase API returned an empty stats payload for '{productId}'.");
    }
}
