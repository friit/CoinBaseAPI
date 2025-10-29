using Coinbase.AdvancedTrade.Authentication;
using Coinbase.AdvancedTrade.DependencyInjection;
using Coinbase.AdvancedTrade.Options;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.DependencyInjection;

public class CoinbaseSetupValidationHostedServiceTests
{
    [Fact]
    public async Task MissingCredentialsProvider_FailsOnStart()
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddCoinbaseAdvancedTradeClient(o =>
                {
                    o.BaseUri = new Uri("https://api.coinbase.com/");
                });
            })
            .Build();

        // No ICoinbaseCredentialsProvider is registered, host start should fail
        await host.Invoking(h => h.StartAsync()).Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task WithOptionsCredentialsProvider_Starts()
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddLogging();
                services.AddCoinbaseAdvancedTradeClient(o =>
                {
                    o.BaseUri = new Uri("https://api.coinbase.com/");
                }).UseOptionsCredentials();

                services.Configure<CoinbaseCredentialsOptions>(o =>
                {
                    o.ApiKey = "key";
                    o.ApiSecret = Convert.ToBase64String("secret"u8.ToArray());
                    o.Passphrase = "pass";
                });
            })
            .Build();

        await host.StartAsync();
        await host.StopAsync();
    }
}

