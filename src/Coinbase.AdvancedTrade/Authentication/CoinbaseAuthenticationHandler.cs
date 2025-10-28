using System.Globalization;
using System.Net.Http.Headers;
using Coinbase.AdvancedTrade.Internal;
using Coinbase.AdvancedTrade.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Coinbase.AdvancedTrade.Authentication;

internal sealed class CoinbaseAuthenticationHandler : DelegatingHandler
{
    private const string ApiKeyHeader = "CB-ACCESS-KEY";
    private const string PassphraseHeader = "CB-ACCESS-PASSPHRASE";
    private const string TimestampHeader = "CB-ACCESS-TIMESTAMP";
    private const string SignatureHeader = "CB-ACCESS-SIGN";
    private const string UserAgentProductName = "CoinbaseAdvancedTradeClient";

    private readonly IOptionsMonitor<CoinbaseAdvancedTradeOptions> _optionsMonitor;
    private readonly ICoinbaseRequestSigner _signer;
    private readonly ISystemClock _clock;
    private readonly ILogger<CoinbaseAuthenticationHandler> _logger;

    public CoinbaseAuthenticationHandler(
        IOptionsMonitor<CoinbaseAdvancedTradeOptions> optionsMonitor,
        ICoinbaseRequestSigner signer,
        ISystemClock clock,
        ILogger<CoinbaseAuthenticationHandler> logger)
    {
        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
        _signer = signer ?? throw new ArgumentNullException(nameof(signer));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is null)
        {
            throw new InvalidOperationException("The Coinbase request must have a RequestUri.");
        }

        var options = _optionsMonitor.CurrentValue ?? throw new InvalidOperationException("Coinbase options are not configured.");
        ValidateOptions(options);

        var timestamp = _clock.UtcNow;
        var timestampText = timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var requestPath = request.RequestUri.PathAndQuery;
        var body = request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        var signature = _signer.CreateSignature(timestamp, request.Method, requestPath, body);

        PrepareHeaders(request.Headers, options);
        request.Headers.TryAddWithoutValidation(ApiKeyHeader, options.ApiKey);
        request.Headers.TryAddWithoutValidation(PassphraseHeader, options.Passphrase);
        request.Headers.TryAddWithoutValidation(TimestampHeader, timestampText);
        request.Headers.TryAddWithoutValidation(SignatureHeader, signature);

        _logger.LogDebug("Coinbase request signed {Method} {Path}", request.Method, requestPath);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static void PrepareHeaders(HttpRequestHeaders headers, CoinbaseAdvancedTradeOptions options)
    {
        if (!headers.UserAgent.Any())
        {
            headers.UserAgent.Add(new ProductInfoHeaderValue(UserAgentProductName, typeof(CoinbaseAuthenticationHandler).Assembly.GetName().Version?.ToString() ?? "1.0"));
        }

        headers.Remove(ApiKeyHeader);
        headers.Remove(PassphraseHeader);
        headers.Remove(TimestampHeader);
        headers.Remove(SignatureHeader);
    }

    private static void ValidateOptions(CoinbaseAdvancedTradeOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.ApiKey))
        {
            throw new InvalidOperationException("Coinbase API key is required.");
        }

        if (string.IsNullOrWhiteSpace(options.Passphrase))
        {
            throw new InvalidOperationException("Coinbase API passphrase is required.");
        }

        if (string.IsNullOrWhiteSpace(options.ApiSecret))
        {
            throw new InvalidOperationException("Coinbase API secret is required.");
        }
    }
}
