using System.Net.Http;
using Coinbase.AdvancedTrade.Models.Accounts;
using Coinbase.AdvancedTrade.Http;
using Microsoft.Extensions.Logging;

namespace Coinbase.AdvancedTrade.Clients.Accounts;

internal sealed class AccountsClient : CoinbaseClientBase, IAccountsClient
{
    public AccountsClient(ICoinbaseHttpClient httpClient, ILogger<AccountsClient> logger)
        : base(httpClient, logger)
    {
    }

    public async Task<AccountsResponse> GetAccountsAsync(string? cursor = null, int? limit = null, CancellationToken cancellationToken = default)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query["cursor"] = cursor;
        }

        if (limit.HasValue)
        {
            query["limit"] = limit.Value.ToString();
        }

        var response = query.Count > 0
            ? await SendWithQueryAsync<AccountsResponse>(HttpMethod.Get, "accounts", query, cancellationToken).ConfigureAwait(false)
            : await SendAsync<AccountsResponse>(HttpMethod.Get, "accounts", cancellationToken).ConfigureAwait(false);

        return response ?? throw new InvalidOperationException("Coinbase API returned an empty accounts payload.");
    }

    public async Task<Account> GetAccountAsync(string accountUuid, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accountUuid);
        var account = await SendAsync<Account>(HttpMethod.Get, $"accounts/{accountUuid}", cancellationToken).ConfigureAwait(false);
        return account ?? throw new InvalidOperationException($"Coinbase API returned an empty account payload for '{accountUuid}'.");
    }
}
