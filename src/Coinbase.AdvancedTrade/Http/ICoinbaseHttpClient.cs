namespace Coinbase.AdvancedTrade.Http;

internal interface ICoinbaseHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
    Task<TResponse?> ReadAsJsonAsync<TResponse>(HttpResponseMessage response, CancellationToken cancellationToken = default);
}
