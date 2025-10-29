using Coinbase.AdvancedTrade.Options;
using Microsoft.Extensions.Options;

namespace Coinbase.AdvancedTrade.Authentication;

internal sealed class OptionsCoinbaseCredentialsProvider : ICoinbaseCredentialsProvider
{
    private readonly IOptionsMonitor<CoinbaseCredentialsOptions> _options;

    public OptionsCoinbaseCredentialsProvider(IOptionsMonitor<CoinbaseCredentialsOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public Task<CoinbaseCredentials> GetAsync(CancellationToken cancellationToken = default)
    {
        var current = _options.CurrentValue;
        if (string.IsNullOrWhiteSpace(current.ApiKey))
        {
            throw new InvalidOperationException("Coinbase API key is required.");
        }

        if (string.IsNullOrWhiteSpace(current.ApiSecret))
        {
            throw new InvalidOperationException("Coinbase API secret is required.");
        }

        if (string.IsNullOrWhiteSpace(current.Passphrase))
        {
            throw new InvalidOperationException("Coinbase API passphrase is required.");
        }

        return Task.FromResult(new CoinbaseCredentials(current.ApiKey, current.ApiSecret, current.Passphrase));
    }
}
