using Coinbase.AdvancedTrade.Clients.Accounts;
using Coinbase.AdvancedTrade.Clients.Orders;
using Coinbase.AdvancedTrade.Clients.Products;

namespace Coinbase.AdvancedTrade.Clients.Core;

internal sealed class CoinbaseAdvancedTradeClient : ICoinbaseAdvancedTradeClient
{
    public CoinbaseAdvancedTradeClient(
        IAccountsClient accountsClient,
        IOrdersClient ordersClient,
        IPublicMarketClient publicMarketClient)
    {
        Accounts = accountsClient ?? throw new ArgumentNullException(nameof(accountsClient));
        Orders = ordersClient ?? throw new ArgumentNullException(nameof(ordersClient));
        Public = publicMarketClient ?? throw new ArgumentNullException(nameof(publicMarketClient));
    }

    public IAccountsClient Accounts { get; }

    public IOrdersClient Orders { get; }

    public IPublicMarketClient Public { get; }
}
