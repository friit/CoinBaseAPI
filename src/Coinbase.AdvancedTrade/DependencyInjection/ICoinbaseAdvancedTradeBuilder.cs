using Microsoft.Extensions.DependencyInjection;

namespace Coinbase.AdvancedTrade.DependencyInjection;

public interface ICoinbaseAdvancedTradeBuilder
{
    IServiceCollection Services { get; }
}

