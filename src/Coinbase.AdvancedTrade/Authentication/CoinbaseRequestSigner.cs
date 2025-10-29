using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Coinbase.AdvancedTrade.Authentication;

internal sealed class CoinbaseRequestSigner : ICoinbaseRequestSigner
{
    public string CreateSignature(string apiSecret, DateTimeOffset timestamp, HttpMethod method, string requestPath, string? body)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiSecret);
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrEmpty(requestPath);

        var timestampString = timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture);
        var upperMethod = method.Method.ToUpperInvariant();
        var payload = body ?? string.Empty;

        var prehash = $"{timestampString}{upperMethod}{requestPath}{payload}";
        var buffer = Encoding.UTF8.GetBytes(prehash);
        var keyBytes = Convert.FromBase64String(apiSecret);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(buffer);

        return Convert.ToBase64String(hash);
    }
}
