using System.ComponentModel.DataAnnotations;

namespace Coinbase.AdvancedTrade.Options;

/// <summary>
/// Configuration settings for the Coinbase Advanced Trade API client.
/// </summary>
public sealed class CoinbaseAdvancedTradeOptions
{
    private static readonly Uri DefaultBaseUri = new("https://api.coinbase.com/");
    private const string DefaultBrokerageApiPath = "api/v3/brokerage/";

    /// <summary>
    /// Gets or sets the base URI for the Coinbase API (defaults to the production REST endpoint).
    /// </summary>
    public Uri BaseUri { get; set; } = DefaultBaseUri;

    /// <summary>
    /// Gets or sets the relative API prefix appended to <see cref="BaseUri"/>.
    /// </summary>
    public string BrokerageApiPath { get; set; } = DefaultBrokerageApiPath;

    /// <summary>
    /// Gets or sets the timeout applied to outbound HTTP requests (defaults to 15 seconds).
    /// </summary>
    public TimeSpan HttpClientTimeout { get; set; } = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Gets the resolved base endpoint for brokerage requests.
    /// </summary>
    public Uri GetBrokerageEndpoint()
    {
        if (!BaseUri.IsAbsoluteUri)
        {
            throw new InvalidOperationException("BaseUri must be absolute.");
        }

        return new Uri(BaseUri, BrokerageApiPath);
    }
}
