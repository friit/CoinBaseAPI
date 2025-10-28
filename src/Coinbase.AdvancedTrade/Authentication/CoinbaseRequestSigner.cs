using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Coinbase.AdvancedTrade.Options;

namespace Coinbase.AdvancedTrade.Authentication;

internal sealed class CoinbaseRequestSigner : ICoinbaseRequestSigner
{
    private readonly byte[] _secretKey;

    public CoinbaseRequestSigner(CoinbaseAdvancedTradeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.ApiSecret))
        {
            throw new ArgumentException("API secret must be provided for request signing.", nameof(options));
        }

        _secretKey = Convert.FromBase64String(options.ApiSecret);
    }

    public string CreateSignature(DateTimeOffset timestamp, HttpMethod method, string requestPath, string? body)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrEmpty(requestPath);

        var timestampString = timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var upperMethod = method.Method.ToUpperInvariant();
        var payload = body ?? string.Empty;

        var prehash = $"{timestampString}{upperMethod}{requestPath}{payload}";
        var buffer = Encoding.UTF8.GetBytes(prehash);
        using var hmac = new HMACSHA256(_secretKey);
        var hash = hmac.ComputeHash(buffer);

        return Convert.ToBase64String(hash);
    }
}
