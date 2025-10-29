namespace Coinbase.AdvancedTrade.Authentication;

public sealed record CoinbaseCredentials(string ApiKey, string ApiSecret, string Passphrase);

public interface ICoinbaseCredentialsProvider
{
    Task<CoinbaseCredentials> GetAsync(CancellationToken cancellationToken = default);
}
