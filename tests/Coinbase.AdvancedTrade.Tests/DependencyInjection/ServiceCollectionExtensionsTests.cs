using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Clients.Core;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Clients.Products;
using Coinbase.AdvancedTrade.DependencyInjection;
using Coinbase.AdvancedTrade.Http;
using Coinbase.AdvancedTrade.Options;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCoinbaseAdvancedTradeClient_RegistersDependencies()
    {
        var services = new ServiceCollection();
        services.AddLogging();

        services.AddCoinbaseAdvancedTradeClient(options =>
        {
            options.BaseUri = new Uri("https://example.com/");
        });

        using var provider = services.BuildServiceProvider();

        provider.GetRequiredService<ICoinbaseAdvancedTradeClient>().Should().NotBeNull();
        provider.GetRequiredService<IAccountsClient>().Should().NotBeNull();
        provider.GetRequiredService<IOrdersClient>().Should().NotBeNull();
        provider.GetRequiredService<IPublicMarketClient>().Should().NotBeNull();
        provider.GetRequiredService<ICoinbaseHttpClient>().Should().NotBeNull();

        var options = provider.GetRequiredService<IOptionsMonitor<CoinbaseAdvancedTradeOptions>>().CurrentValue;
        options.BaseUri.Should().Be(new Uri("https://example.com/"));
    }
}
