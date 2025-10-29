using System.Net.Http;
using Coinbase.AdvancedTrade.Clients.Products;
using Coinbase.AdvancedTrade.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Clients.Products;

public class PublicMarketClientTests
{
    [Fact]
    public async Task GetProductsAsync_ReturnsCollection()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Get, "https://localhost/products")
            .WithQueryString("product_type", "SPOT")
            .Respond("application/json", "{\"products\":[{\"product_id\":\"BTC-USD\",\"base_currency\":\"BTC\",\"quote_currency\":\"USD\",\"display_name\":\"BTC/USD\",\"status\":\"online\",\"price_increment\":0.01,\"base_increment\":0.00000001,\"quote_increment\":0.01}],\"num_products\":1}");

        var client = new PublicMarketClient(httpClient, NullLogger<PublicMarketClient>.Instance);

        var response = await client.GetProductsAsync("SPOT");

        response.Products.Should().HaveCount(1);
        response.Products[0].ProductId.Should().Be("BTC-USD");
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetProductBookAsync_UsesLimit()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Get, "https://localhost/products/BTC-USD/book")
            .WithQueryString("limit", "5")
            .Respond("application/json", "{\"product_id\":\"BTC-USD\",\"bids\":[],\"asks\":[],\"sequence\":123}");

        var client = new PublicMarketClient(httpClient, NullLogger<PublicMarketClient>.Instance);

        var book = await client.GetProductBookAsync("BTC-USD", 5);

        book.ProductId.Should().Be("BTC-USD");
        handler.VerifyNoOutstandingExpectation();
    }
}
