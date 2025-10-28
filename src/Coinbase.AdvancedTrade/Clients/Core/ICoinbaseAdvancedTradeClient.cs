using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Clients.Products;

namespace Coinbase.AdvancedTrade.Clients.Core;

public interface ICoinbaseAdvancedTradeClient
{
    IAccountsClient Accounts { get; }
    IOrdersClient Orders { get; }
    IPublicMarketClient Public { get; }
}
