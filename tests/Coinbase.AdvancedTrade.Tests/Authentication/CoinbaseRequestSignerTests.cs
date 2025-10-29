using System.Globalization;
using Coinbase.AdvancedTrade.Authentication;
using FluentAssertions;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Authentication;

public class CoinbaseRequestSignerTests
{
    [Fact]
    public void CreateSignature_GeneratesExpectedHash()
    {
        // Arrange sample values from docs for determinism.
        var apiSecret = Convert.ToBase64String("secret"u8.ToArray());
        var signer = new CoinbaseRequestSigner();

        var timestamp = DateTimeOffset.FromUnixTimeSeconds(1700000000);
        var method = HttpMethod.Post;
        var requestPath = "/api/v3/brokerage/orders";
        var body = "{\"test\":true}";

        // Act
        var signature = signer.CreateSignature(apiSecret, timestamp, method, requestPath, body);

        // Assert
        signature.Should().Be("2ez5Ta8MUQ2A14wSeb/VJdNhBk2H62w3SZCiAGSg+PM=");
    }

    [Fact]
    public void CreateSignature_WithMissingSecret_ThrowsArgumentException()
    {
        var signer = new CoinbaseRequestSigner();
        var act = () => signer.CreateSignature(null!, DateTimeOffset.UtcNow, HttpMethod.Get, "/", null);
        act.Should().Throw<ArgumentException>()
           .WithParameterName("apiSecret");
    }
}
