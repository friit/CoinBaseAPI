namespace Coinbase.AdvancedTrade.Internal;

internal sealed class UtcSystemClock : ISystemClock
{
    public static readonly UtcSystemClock Instance = new();

    private UtcSystemClock()
    {
    }

    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
