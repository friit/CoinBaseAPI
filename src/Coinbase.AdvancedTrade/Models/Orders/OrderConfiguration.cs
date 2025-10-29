using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

public sealed record OrderConfiguration(
    [property: JsonPropertyName("market_market_ioc")] MarketOrderConfiguration? MarketMarketIoc,
    [property: JsonPropertyName("limit_limit_gtc")] LimitOrderConfiguration? LimitLimitGtc);
