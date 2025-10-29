using System.Net.Http;
using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Clients.Accounts;

public class AccountsClientTests
{
    [Fact]
    public async Task GetAccountsAsync_UsesQueryParameters()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Get, "https://localhost/accounts")
            .WithQueryString("cursor", "abc")
            .WithQueryString("limit", "10")
            .Respond("application/json", "{\"accounts\":[],\"has_next\":false}");

        var client = new AccountsClient(httpClient, NullLogger<AccountsClient>.Instance);

        var response = await client.GetAccountsAsync("abc", 10);

        response.Should().NotBeNull();
        response.HasNext.Should().BeFalse();
        handler.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetAccountAsync_ReturnsAccount()
    {
        var (handler, httpClient) = TestHttpClientFactory.Create();
        handler.Expect(HttpMethod.Get, "https://localhost/accounts/test-account")
            .Respond("application/json", "{\"uuid\":\"test-account\",\"name\":\"BTC Wallet\",\"currency\":\"BTC\",\"available_balance\":{\"value\":0.5,\"currency\":\"BTC\"},\"type\":\"WALLET\",\"ready\":true,\"default\":false,\"active\":true,\"hold\":{\"value\":0,\"currency\":\"BTC\"},\"created_at\":\"2024-01-01T00:00:00Z\",\"updated_at\":\"2024-01-02T00:00:00Z\"}");

        var client = new AccountsClient(httpClient, NullLogger<AccountsClient>.Instance);

        var account = await client.GetAccountAsync("test-account");

        account.Uuid.Should().Be("test-account");
        account.Currency.Should().Be("BTC");
        handler.VerifyNoOutstandingExpectation();
    }
}
