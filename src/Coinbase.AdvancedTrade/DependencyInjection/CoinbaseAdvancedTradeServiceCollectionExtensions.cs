using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Clients.Core;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Clients.Products;
using Coinbase.AdvancedTrade.Authentication;
using Coinbase.AdvancedTrade.Http;
using Coinbase.AdvancedTrade.Internal;
using Coinbase.AdvancedTrade.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Coinbase.AdvancedTrade.DependencyInjection;

public static class CoinbaseAdvancedTradeServiceCollectionExtensions
{
    private const int DefaultRetryCount = 3;

    public static IServiceCollection AddCoinbaseAdvancedTradeClient(this IServiceCollection services, Action<CoinbaseAdvancedTradeOptions> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configureOptions);

        services.AddOptions<CoinbaseAdvancedTradeOptions>()
            .Configure(configureOptions)
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.ApiKey), "API key is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ApiSecret), "API secret is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Passphrase), "API passphrase is required.");

        services.TryAddSingleton<ISystemClock>(_ => UtcSystemClock.Instance);
        services.TryAddSingleton<ICoinbaseRequestSigner>(sp =>
        {
            var opts = sp.GetRequiredService<IOptionsMonitor<CoinbaseAdvancedTradeOptions>>().CurrentValue;
            return new CoinbaseRequestSigner(opts);
        });

        services.AddTransient<CoinbaseAuthenticationHandler>();

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

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(response => response.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(DefaultRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
