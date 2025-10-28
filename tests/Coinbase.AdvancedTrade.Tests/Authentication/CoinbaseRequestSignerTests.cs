using System.Globalization;
using Coinbase.AdvancedTrade.Authentication;
using Coinbase.AdvancedTrade.Options;
using FluentAssertions;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Authentication;

public class CoinbaseRequestSignerTests
{
    [Fact]
    public void CreateSignature_GeneratesExpectedHash()
    {
        // Arrange sample values from docs for determinism.
        var options = new CoinbaseAdvancedTradeOptions
        {
            ApiSecret = Convert.ToBase64String("secret"u8.ToArray())
        };
        var signer = new CoinbaseRequestSigner(options);

        var timestamp = DateTimeOffset.FromUnixTimeSeconds(1700000000);
        var method = HttpMethod.Post;
        var requestPath = "/api/v3/brokerage/orders";
        var body = "{\"test\":true}";

        // Act
        var signature = signer.CreateSignature(timestamp, method, requestPath, body);

        // Assert
        signature.Should().Be("2ez5Ta8MUQ2A14wSeb/VJdNhBk2H62w3SZCiAGSg+PM=");
    }

    [Fact]
    public void Ctor_WithMissingSecret_ThrowsArgumentException()
    {
        var options = new CoinbaseAdvancedTradeOptions { ApiSecret = null };
        var act = () => new CoinbaseRequestSigner(options);
        act.Should().Throw<ArgumentException>()
           .WithMessage("*API secret must be provided*");
    }
}
