using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Models.Orders;
using Coinbase.AdvancedTrade.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Clients.Orders;

public class OrdersClientTests
{
    [Fact]
    public async Task CreateOrderAsync_SendsExpectedPayload()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Post, "https://localhost/orders")
            .With(message =>
            {
                var json = message.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                using var doc = JsonDocument.Parse(json);
                doc.RootElement.GetProperty("client_order_id").GetString().Should().Be("client-1");
                var limitPrice = doc.RootElement.GetProperty("order_configuration").GetProperty("limit_limit_gtc").GetProperty("limit_price").GetString();
                decimal.Parse(limitPrice!, CultureInfo.InvariantCulture).Should().Be(1000m);
                return true;
            })
            .Respond("application/json", "{\"success\":true,\"order_id\":\"order-1\"}");

        var client = new OrdersClient(httpClient, NullLogger<OrdersClient>.Instance);
        var request = new CreateOrderRequest(
            "client-1",
            "BTC-USD",
            OrderSide.Buy,
            new OrderConfiguration(null, new LimitOrderConfiguration(0.01m, 1000m, true)),
            null);

        var response = await client.CreateOrderAsync(request);

        response.Success.Should().BeTrue();
        response.OrderId.Should().Be("order-1");
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task ListOrdersAsync_AppendsQueryParameters()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Get, "https://localhost/orders/historical/batch")
            .WithQueryString("product_id", "BTC-USD")
            .WithQueryString("cursor", "cursor123")
            .WithQueryString("limit", "50")
            .Respond("application/json", "{\"orders\":[],\"has_next\":false}");

        var client = new OrdersClient(httpClient, NullLogger<OrdersClient>.Instance);

        var result = await client.ListOrdersAsync("BTC-USD", "cursor123", 50);

        result.HasNext.Should().BeFalse();
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CancelOrdersAsync_SendsIdentifiers()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Post, "https://localhost/orders/batch_cancel")
            .With(message =>
            {
                var json = message.Content!.ReadAsStringAsync().GetAwaiter().GetResult();
                using var doc = JsonDocument.Parse(json);
                doc.RootElement.GetProperty("order_ids").GetArrayLength().Should().Be(1);
                doc.RootElement.GetProperty("client_order_ids").GetArrayLength().Should().Be(1);
                return true;
            })
            .Respond("application/json", "{\"results\":[{\"order_id\":\"order-1\",\"success\":true}]} ");

        var client = new OrdersClient(httpClient, NullLogger<OrdersClient>.Instance);

        var response = await client.CancelOrdersAsync(new CancelOrdersRequest(new[] { "order-1" }, new[] { "client-1" }));

        response.Results.Should().HaveCount(1);
        response.Results[0].Success.Should().BeTrue();
        handler.VerifyNoOutstandingExpectation();
    }
}
