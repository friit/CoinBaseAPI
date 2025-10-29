using Microsoft.Extensions.DependencyInjection;

namespace Coinbase.AdvancedTrade.DependencyInjection;

/// <summary>
/// A builder used to configure the Coinbase Advanced Trade client registration.
/// </summary>
public interface ICoinbaseAdvancedTradeBuilder
{
    /// <summary>
    /// The underlying service collection to which registrations are applied.
    /// </summary>
    IServiceCollection Services { get; }
}
