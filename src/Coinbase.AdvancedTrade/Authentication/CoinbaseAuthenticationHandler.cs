using System.Globalization;
using System.Net.Http.Headers;
using Coinbase.AdvancedTrade.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Coinbase.AdvancedTrade.Authentication;

internal sealed class CoinbaseAuthenticationHandler : DelegatingHandler
{
    private const string ApiKeyHeader = "CB-ACCESS-KEY";
    private const string PassphraseHeader = "CB-ACCESS-PASSPHRASE";
    private const string TimestampHeader = "CB-ACCESS-TIMESTAMP";
    private const string SignatureHeader = "CB-ACCESS-SIGN";
    private const string UserAgentProductName = "CoinbaseAdvancedTradeClient";

    private readonly IServiceProvider _serviceProvider;
    private readonly ICoinbaseRequestSigner _signer;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<CoinbaseAuthenticationHandler> _logger;

    public CoinbaseAuthenticationHandler(
        IServiceProvider serviceProvider,
        ICoinbaseRequestSigner signer,
        TimeProvider timeProvider,
        ILogger<CoinbaseAuthenticationHandler> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _signer = signer ?? throw new ArgumentNullException(nameof(signer));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is null)
        {
            throw new InvalidOperationException("The Coinbase request must have a RequestUri.");
        }

        using var scope = _serviceProvider.CreateScope();
        var credentialsProvider = scope.ServiceProvider.GetRequiredService<ICoinbaseCredentialsProvider>();
        var credentials = await credentialsProvider.GetAsync(cancellationToken).ConfigureAwait(false);

        var timestamp = _timeProvider.GetUtcNow();
        var timestampText = timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var requestPath = request.RequestUri.PathAndQuery;
        var body = request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

        var signature = _signer.CreateSignature(credentials.ApiSecret, timestamp, request.Method, requestPath, body);

        PrepareHeaders(request.Headers);
        request.Headers.TryAddWithoutValidation(ApiKeyHeader, credentials.ApiKey);
        request.Headers.TryAddWithoutValidation(PassphraseHeader, credentials.Passphrase);
        request.Headers.TryAddWithoutValidation(TimestampHeader, timestampText);
        request.Headers.TryAddWithoutValidation(SignatureHeader, signature);

        _logger.LogDebug("Coinbase request signed {Method} {Path}", request.Method, requestPath);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }

    private static void PrepareHeaders(HttpRequestHeaders headers)
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

    
}
