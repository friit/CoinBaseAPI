using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Coinbase.AdvancedTrade.Errors;
using Coinbase.AdvancedTrade.Http;
using Coinbase.AdvancedTrade.Serialization;
using Microsoft.Extensions.Logging;

namespace Coinbase.AdvancedTrade.Clients;

internal abstract class CoinbaseClientBase
{
    private readonly ICoinbaseHttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    protected readonly ILogger Logger;

    protected CoinbaseClientBase(ICoinbaseHttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serializerOptions = CoinbaseSerializerOptions.Create();
    }

    protected async Task<TResponse?> SendAsync<TRequest, TResponse>(
        HttpMethod method,
        string path,
        TRequest? request,
        IDictionary<string, string?>? queryParameters,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrEmpty(path);

        var requestUri = BuildRequestUri(path, queryParameters);

        using var message = new HttpRequestMessage(method, requestUri);
        if (request is not null)
        {
            message.Content = JsonContent.Create(request, options: _serializerOptions);
        }

        using var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw await CoinbaseApiException.CreateAsync(response, _serializerOptions, cancellationToken).ConfigureAwait(false);
        }

        if (typeof(TResponse) == typeof(VoidResponse))
        {
            return default;
        }

        return await _httpClient.ReadAsJsonAsync<TResponse>(response, cancellationToken).ConfigureAwait(false);
    }

    protected Task SendAsync(HttpMethod method, string path, CancellationToken cancellationToken)
        => SendAsync<object, VoidResponse>(method, path, null, null, cancellationToken);

    protected Task<TResponse?> SendAsync<TResponse>(HttpMethod method, string path, CancellationToken cancellationToken)
        => SendAsync<object, TResponse>(method, path, null, null, cancellationToken);

    protected Task<TResponse?> SendAsync<TRequest, TResponse>(HttpMethod method, string path, TRequest? request, CancellationToken cancellationToken)
        => SendAsync<TRequest, TResponse>(method, path, request, null, cancellationToken);

    protected Task<TResponse?> SendWithQueryAsync<TResponse>(HttpMethod method, string path, IDictionary<string, string?> queryParameters, CancellationToken cancellationToken)
        => SendAsync<object, TResponse>(method, path, null, queryParameters, cancellationToken);

    private static string BuildRequestUri(string path, IDictionary<string, string?>? queryParameters)
    {
        if (queryParameters is null || queryParameters.Count == 0)
        {
            return path;
        }

        var builder = new StringBuilder(path);
        builder.Append(path.Contains('?') ? '&' : '?');

        var first = true;
        foreach (var kvp in queryParameters)
        {
            if (string.IsNullOrEmpty(kvp.Value))
            {
                continue;
            }

            if (!first)
            {
                builder.Append('&');
            }

            builder.Append(Uri.EscapeDataString(kvp.Key));
            builder.Append('=');
            builder.Append(Uri.EscapeDataString(kvp.Value));
            first = false;
        }

        return builder.ToString();
    }

    protected record VoidResponse;
}
