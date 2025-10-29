using System;
using Coinbase.AdvancedTrade.Http;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Coinbase.AdvancedTrade.Tests.Helpers;

internal static class TestHttpClientFactory
{
    public static (MockHttpMessageHandler Handler, ICoinbaseHttpClient Client) Create()
    {
        var handler = new MockHttpMessageHandler();
        var httpClient = handler.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://localhost/");
        var client = new CoinbaseHttpClient(httpClient, NullLogger<CoinbaseHttpClient>.Instance);
        return (handler, client);
    }
}
