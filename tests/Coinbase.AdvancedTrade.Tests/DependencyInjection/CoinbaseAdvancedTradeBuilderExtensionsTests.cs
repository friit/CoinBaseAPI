using Coinbase.AdvancedTrade.Authentication;
using Coinbase.AdvancedTrade.DependencyInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.DependencyInjection;

public class CoinbaseAdvancedTradeBuilderExtensionsTests
{
    private sealed class CustomProvider : ICoinbaseCredentialsProvider
    {
        public Task<CoinbaseCredentials> GetAsync(CancellationToken cancellationToken = default)
            => Task.FromResult(new CoinbaseCredentials("k","s","p"));
    }

    [Fact]
    public void UseOptionsCredentials_RegistersProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddCoinbaseAdvancedTradeClient(o => { }).UseOptionsCredentials();

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<ICoinbaseCredentialsProvider>()
            .Should().BeOfType<OptionsCoinbaseCredentialsProvider>();
    }

    [Fact]
    public void UseCredentialsProvider_ReplacesRegistration()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        var builder = services.AddCoinbaseAdvancedTradeClient(o => { }).UseOptionsCredentials();
        builder.UseCredentialsProvider<CustomProvider>();

        using var provider = services.BuildServiceProvider();
        provider.GetRequiredService<ICoinbaseCredentialsProvider>()
            .Should().BeOfType<CustomProvider>();
    }
}

