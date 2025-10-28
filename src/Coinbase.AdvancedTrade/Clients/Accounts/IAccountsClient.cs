using Coinbase.AdvancedTrade.Models.Accounts;

namespace Coinbase.AdvancedTrade.Clients.Accounts;

public interface IAccountsClient
{
    Task<AccountsResponse> GetAccountsAsync(string? cursor = null, int? limit = null, CancellationToken cancellationToken = default);
    Task<Account> GetAccountAsync(string accountUuid, CancellationToken cancellationToken = default);
}
