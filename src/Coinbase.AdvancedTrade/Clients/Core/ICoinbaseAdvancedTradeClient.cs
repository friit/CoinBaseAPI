using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Clients.Products;

namespace Coinbase.AdvancedTrade.Clients.Core;

/// <summary>
/// Aggregates access to Coinbase Advanced Trade endpoint groups.
/// </summary>
public interface ICoinbaseAdvancedTradeClient
{
    /// <summary>Exposes account-related endpoints.</summary>
    IAccountsClient Accounts { get; }
    /// <summary>Exposes order-related endpoints.</summary>
    IOrdersClient Orders { get; }
    /// <summary>Exposes public market data endpoints.</summary>
    IPublicMarketClient Public { get; }
}
