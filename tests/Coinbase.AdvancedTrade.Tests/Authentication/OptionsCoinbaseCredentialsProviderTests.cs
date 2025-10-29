using Coinbase.AdvancedTrade.Authentication;
using Coinbase.AdvancedTrade.Options;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Authentication;

public class OptionsCoinbaseCredentialsProviderTests
{
    [Fact]
    public async Task GetAsync_ReturnsConfiguredCredentials()
    {
        var services = new ServiceCollection();
        services.AddOptions<CoinbaseCredentialsOptions>().Configure(o =>
        {
            o.ApiKey = "key";
            o.ApiSecret = Convert.ToBase64String("secret"u8.ToArray());
            o.Passphrase = "pass";
        });
        services.AddSingleton<OptionsCoinbaseCredentialsProvider>();

        using var provider = services.BuildServiceProvider();
        var providerImpl = provider.GetRequiredService<OptionsCoinbaseCredentialsProvider>();

        var creds = await providerImpl.GetAsync();

        creds.ApiKey.Should().Be("key");
        creds.ApiSecret.Should().NotBeNullOrWhiteSpace();
        creds.Passphrase.Should().Be("pass");
    }

    [Fact]
    public async Task GetAsync_MissingValues_Throws()
    {
        var services = new ServiceCollection();
        services.AddOptions<CoinbaseCredentialsOptions>();
        services.AddSingleton<OptionsCoinbaseCredentialsProvider>();

        using var provider = services.BuildServiceProvider();
        var providerImpl = provider.GetRequiredService<OptionsCoinbaseCredentialsProvider>();

        await FluentActions.Invoking(() => providerImpl.GetAsync()).Should().ThrowAsync<InvalidOperationException>();
    }
}
