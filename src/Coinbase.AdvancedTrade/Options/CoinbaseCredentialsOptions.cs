using System.ComponentModel.DataAnnotations;

namespace Coinbase.AdvancedTrade.Options;

/// <summary>
/// Credentials used to authenticate Coinbase Advanced Trade requests.
/// </summary>
public sealed class CoinbaseCredentialsOptions
{
    /// <summary>
    /// The Coinbase API key.
    /// </summary>
    [Required]
    public string? ApiKey { get; set; }

    /// <summary>
    /// The API secret used for HMAC signing (base64 encoded).
    /// </summary>
    [Required]
    public string? ApiSecret { get; set; }

    /// <summary>
    /// The Coinbase API passphrase.
    /// </summary>
    [Required]
    public string? Passphrase { get; set; }
}
