using System.Text.Json.Serialization;
using Coinbase.AdvancedTrade.Models.Common;

namespace Coinbase.AdvancedTrade.Models.Accounts;

public sealed record Account(
    [property: JsonPropertyName("uuid")] string Uuid,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("currency")] string Currency,
    [property: JsonPropertyName("available_balance")] MoneyAmount AvailableBalance,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("ready")] bool Ready,
    [property: JsonPropertyName("default")] bool Default,
    [property: JsonPropertyName("active")] bool Active,
    [property: JsonPropertyName("hold")] MoneyAmount Hold,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTimeOffset UpdatedAt);
