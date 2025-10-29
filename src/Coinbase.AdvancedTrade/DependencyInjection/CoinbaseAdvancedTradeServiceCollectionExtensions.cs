using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Clients.Core;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Clients.Products;
using Coinbase.AdvancedTrade.Authentication;
using Coinbase.AdvancedTrade.Http;
using Coinbase.AdvancedTrade.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Coinbase.AdvancedTrade.DependencyInjection;

/// <summary>
/// Dependency injection helpers for registering the Coinbase Advanced Trade client.
/// </summary>
public static class CoinbaseAdvancedTradeServiceCollectionExtensions
{
    private const int DefaultRetryCount = 3;

    /// <summary>
    /// Registers the Coinbase Advanced Trade client and supporting services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Callback to configure client behavior options.</param>
    /// <returns>A builder that can be used to configure credentials.</returns>
    /// <remarks>
    /// This method does not register credentials by default. Consumers must configure
    /// an <see cref="Coinbase.AdvancedTrade.Authentication.ICoinbaseCredentialsProvider"/> via the returned builder
    /// (for example by calling <c>UseOptionsCredentials()</c>) or by registering a custom provider.
    /// Hosted applications will fail on startup if a provider is not registered.
    /// </remarks>
    public static ICoinbaseAdvancedTradeBuilder AddCoinbaseAdvancedTradeClient(this IServiceCollection services, Action<CoinbaseAdvancedTradeOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);
        
        services.AddOptions<CoinbaseAdvancedTradeOptions>()
            .Configure(configureOptions)
            .ValidateDataAnnotations();

        services.TryAddSingleton<TimeProvider>(_ => TimeProvider.System);
        services.TryAddSingleton<ICoinbaseRequestSigner, CoinbaseRequestSigner>();
        services.AddHostedService<CoinbaseSetupValidationHostedService>();

        services.AddTransient<CoinbaseAuthenticationHandler>();

        //TODO: Review Implementation
        services.AddHttpClient<ICoinbaseHttpClient, CoinbaseHttpClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptionsMonitor<CoinbaseAdvancedTradeOptions>>().CurrentValue;
                client.BaseAddress = options.GetBrokerageEndpoint();
                client.Timeout = options.HttpClientTimeout;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            })
            .AddHttpMessageHandler<CoinbaseAuthenticationHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

        services.TryAddSingleton<IAccountsClient, AccountsClient>();
        services.TryAddSingleton<IOrdersClient, OrdersClient>();
        services.TryAddSingleton<IPublicMarketClient, PublicMarketClient>();
        services.TryAddSingleton<ICoinbaseAdvancedTradeClient, CoinbaseAdvancedTradeClient>();

        return new CoinbaseAdvancedTradeBuilder(services);
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => response.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(DefaultRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
