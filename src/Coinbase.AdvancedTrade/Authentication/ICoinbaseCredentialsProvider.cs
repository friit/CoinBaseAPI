namespace Coinbase.AdvancedTrade.Authentication;

/// <summary>
/// Immutable container for Coinbase API credentials used to sign requests.
/// </summary>
/// <param name="ApiKey">The Coinbase API key.</param>
/// <param name="ApiSecret">The base64-encoded API secret used for HMAC signing.</param>
/// <param name="Passphrase">The Coinbase API passphrase.</param>
public sealed record CoinbaseCredentials(string ApiKey, string ApiSecret, string Passphrase);

/// <summary>
/// Supplies credentials for authenticating Coinbase Advanced Trade requests.
/// Implementations may source credentials from configuration, a per-user context,
/// or any external secret store.
/// </summary>
public interface ICoinbaseCredentialsProvider
{
    /// <summary>
    /// Gets the credentials to use for the current request.
    /// </summary>
    /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
    /// <returns>The credentials containing the API key, secret, and passphrase.</returns>
    Task<CoinbaseCredentials> GetAsync(CancellationToken cancellationToken = default);
}
