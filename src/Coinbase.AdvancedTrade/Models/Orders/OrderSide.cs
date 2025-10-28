using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

[JsonConverter(typeof(OrderSideJsonConverter))]
public enum OrderSide
{
    Buy,
    Sell
}
