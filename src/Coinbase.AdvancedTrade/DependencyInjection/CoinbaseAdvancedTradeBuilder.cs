using Microsoft.Extensions.DependencyInjection;

namespace Coinbase.AdvancedTrade.DependencyInjection;

internal sealed class CoinbaseAdvancedTradeBuilder(IServiceCollection services) : ICoinbaseAdvancedTradeBuilder
{
    public IServiceCollection Services { get; } = services ?? throw new ArgumentNullException(nameof(services));
}

