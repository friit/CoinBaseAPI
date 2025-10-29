using Coinbase.AdvancedTrade.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.AdvancedTrade.DependencyInjection;

public static class CoinbaseAdvancedTradeBuilderExtensions
{
    public static ICoinbaseAdvancedTradeBuilder UseCredentialsProvider<TProvider>(
        this ICoinbaseAdvancedTradeBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProvider : class, ICoinbaseCredentialsProvider
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.Replace(ServiceDescriptor.Describe(typeof(ICoinbaseCredentialsProvider), typeof(TProvider), lifetime));
        return builder;
    }

    public static ICoinbaseAdvancedTradeBuilder UseOptionsCredentials(
        this ICoinbaseAdvancedTradeBuilder builder)
        => builder.UseCredentialsProvider<OptionsCoinbaseCredentialsProvider>(ServiceLifetime.Scoped);
}

