using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Coinbase.AdvancedTrade.Authentication;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Coinbase.AdvancedTrade.Tests.Authentication;

public class CoinbaseAuthenticationHandlerTests
{
    private sealed class FixedTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcNow;
        public FixedTimeProvider(DateTimeOffset utcNow) => _utcNow = utcNow;
        public override DateTimeOffset GetUtcNow() => _utcNow;
    }

    private sealed class TestCredentialsProvider : ICoinbaseCredentialsProvider
    {
        private readonly CoinbaseCredentials _creds;
        public TestCredentialsProvider(CoinbaseCredentials creds) => _creds = creds;
        public Task<CoinbaseCredentials> GetAsync(CancellationToken cancellationToken = default) => Task.FromResult(_creds);
    }

    private sealed class TerminalHandler : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    [Fact]
    public async Task Handler_AddsExpectedHeaders()
    {
        var creds = new CoinbaseCredentials("key", Convert.ToBase64String("secret"u8.ToArray()), "pass");
        var services = new ServiceCollection();
        services.AddSingleton<ICoinbaseCredentialsProvider>(new TestCredentialsProvider(creds));
        using var sp = services.BuildServiceProvider();

        var signer = new CoinbaseRequestSigner();
        var time = new FixedTimeProvider(DateTimeOffset.FromUnixTimeSeconds(1700000000));
        var handler = new CoinbaseAuthenticationHandler(sp, signer, time, NullLogger<CoinbaseAuthenticationHandler>.Instance)
        {
            InnerHandler = new TerminalHandler()
        };

        var http = new HttpClient(handler) { BaseAddress = new Uri("https://localhost/") };

        var response = await http.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/api/v3/brokerage/products"));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var terminal = (TerminalHandler)handler.InnerHandler!;
        terminal.LastRequest.Should().NotBeNull();

        var headers = terminal.LastRequest!.Headers;
        headers.TryGetValues("CB-ACCESS-KEY", out var keyValues).Should().BeTrue();
        keyValues!.Single().Should().Be("key");
        headers.TryGetValues("CB-ACCESS-PASSPHRASE", out var passValues).Should().BeTrue();
        passValues!.Single().Should().Be("pass");
        headers.TryGetValues("CB-ACCESS-TIMESTAMP", out var tsValues).Should().BeTrue();
        tsValues!.Single().Should().Be("1700000000");
        headers.TryGetValues("CB-ACCESS-SIGN", out var signValues).Should().BeTrue();
        signValues!.Single().Should().NotBeNullOrWhiteSpace();

        // User-Agent should be present
        headers.UserAgent.Any().Should().BeTrue();
    }
}

