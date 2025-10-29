using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Accounts;

public sealed record AccountsResponse(
    [property: JsonPropertyName("accounts")] IReadOnlyList<Account> Accounts,
    [property: JsonPropertyName("has_next")] bool HasNext,
    [property: JsonPropertyName("cursor")] string? Cursor);
