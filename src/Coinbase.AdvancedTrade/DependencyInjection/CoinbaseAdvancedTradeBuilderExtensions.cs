using Coinbase.AdvancedTrade.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Coinbase.AdvancedTrade.DependencyInjection;

/// <summary>
/// Extension methods for configuring credentials on the Coinbase client builder.
/// </summary>
public static class CoinbaseAdvancedTradeBuilderExtensions
{
    /// <summary>
    /// Registers a custom <see cref="ICoinbaseCredentialsProvider"/> implementation.
    /// </summary>
    /// <typeparam name="TProvider">The provider type to register.</typeparam>
    /// <param name="builder">The Coinbase client builder.</param>
    /// <param name="lifetime">The service lifetime for the provider (defaults to scoped).</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static ICoinbaseAdvancedTradeBuilder UseCredentialsProvider<TProvider>(
        this ICoinbaseAdvancedTradeBuilder builder,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
        where TProvider : class, ICoinbaseCredentialsProvider
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.Replace(ServiceDescriptor.Describe(typeof(ICoinbaseCredentialsProvider), typeof(TProvider), lifetime));
        return builder;
    }

    /// <summary>
    /// Configures the client to use an options-backed credentials provider.
    /// The application must bind <see cref="Options.CoinbaseCredentialsOptions"/>.
    /// </summary>
    /// <param name="builder">The Coinbase client builder.</param>
    /// <returns>The same builder instance for chaining.</returns>
    public static ICoinbaseAdvancedTradeBuilder UseOptionsCredentials(
        this ICoinbaseAdvancedTradeBuilder builder)
        => builder.UseCredentialsProvider<OptionsCoinbaseCredentialsProvider>(ServiceLifetime.Scoped);
}
