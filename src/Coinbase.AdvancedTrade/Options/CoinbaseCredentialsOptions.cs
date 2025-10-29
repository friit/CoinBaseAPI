using System.ComponentModel.DataAnnotations;

namespace Coinbase.AdvancedTrade.Options;

/// <summary>
/// Credentials used to authenticate Coinbase Advanced Trade requests.
/// </summary>
public sealed class CoinbaseCredentialsOptions
{
    [Required]
    public string? ApiKey { get; set; }

    [Required]
    public string? ApiSecret { get; set; }

    [Required]
    public string? Passphrase { get; set; }
}

