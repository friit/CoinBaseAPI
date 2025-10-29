using Coinbase.AdvancedTrade.Models.Accounts;

namespace Coinbase.AdvancedTrade.Clients.Accounts;

/// <summary>
/// Accounts endpoints for Coinbase Advanced Trade.
/// </summary>
public interface IAccountsClient
{
    /// <summary>
    /// Lists accounts with optional pagination.
    /// </summary>
    /// <param name="cursor">An opaque pagination cursor.</param>
    /// <param name="limit">The maximum number of items to return.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<AccountsResponse> GetAccountsAsync(string? cursor = null, int? limit = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a single account by UUID.
    /// </summary>
    /// <param name="accountUuid">The account UUID.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<Account> GetAccountAsync(string accountUuid, CancellationToken cancellationToken = default);
}
