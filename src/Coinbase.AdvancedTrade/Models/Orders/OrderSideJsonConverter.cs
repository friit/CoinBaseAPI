using System.Text.Json;
using System.Text.Json.Serialization;

namespace Coinbase.AdvancedTrade.Models.Orders;

internal sealed class OrderSideJsonConverter : JsonConverter<OrderSide>
{
    public override OrderSide Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string for order side");
        }

        var value = reader.GetString();
        return value switch
        {
            "BUY" => OrderSide.Buy,
            "SELL" => OrderSide.Sell,
            _ => throw new JsonException($"Unsupported order side '{value}'")
        };
    }

    public override void Write(Utf8JsonWriter writer, OrderSide value, JsonSerializerOptions options)
    {
        var text = value switch
        {
            OrderSide.Buy => "BUY",
            OrderSide.Sell => "SELL",
            _ => throw new JsonException($"Unsupported order side '{value}'")
        };

        writer.WriteStringValue(text);
    }
}
