using System.Text.Json.Serialization;
using Coinbase.AdvancedTrade.Models.Common;

namespace Coinbase.AdvancedTrade.Models.Accounts;

public sealed record AccountBalance(
    [property: JsonPropertyName("available_balance")] MoneyAmount AvailableBalance,
    [property: JsonPropertyName("hold")] MoneyAmount Hold);
