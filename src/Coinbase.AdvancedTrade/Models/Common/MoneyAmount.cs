using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Common;

public sealed record MoneyAmount(
    [property: JsonPropertyName("value")] decimal Value,
    [property: JsonPropertyName("currency")] string Currency);
