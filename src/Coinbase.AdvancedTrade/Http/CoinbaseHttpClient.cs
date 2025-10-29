using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Coinbase.AdvancedTrade.Serialization;
using Microsoft.Extensions.Logging;

namespace Coinbase.AdvancedTrade.Http;

internal sealed class CoinbaseHttpClient : ICoinbaseHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<CoinbaseHttpClient> _logger;

    public CoinbaseHttpClient(HttpClient httpClient, ILogger<CoinbaseHttpClient> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serializerOptions = CoinbaseSerializerOptions.Create();
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogDebug("Sending Coinbase request {Method} {Uri}", request.Method, request.RequestUri);
        return _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    }

    public async Task<TResponse?> ReadAsJsonAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(response);

        if (response.Content == null)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>(_serializerOptions, cancellationToken).ConfigureAwait(false);
    }
}
