namespace Coinbase.AdvancedTrade.Internal;

/// <summary>
/// Abstraction over system clock to make time-based logic testable.
/// </summary>
internal interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
}
