using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Coinbase.AdvancedTrade.Authentication;

/// <summary>
/// Default HMAC-SHA256 request signer for Coinbase Advanced Trade requests.
/// Stateless: the API secret is supplied per call.
/// </summary>
internal sealed class CoinbaseRequestSigner : ICoinbaseRequestSigner
{
    /// <summary>
    /// Creates a base64-encoded HMAC-SHA256 signature for the given request.
    /// </summary>
    /// <param name="apiSecret">The base64-encoded API secret.</param>
    /// <param name="timestamp">The UTC timestamp to include in the signature.</param>
    /// <param name="method">The HTTP method.</param>
    /// <param name="requestPath">The request path and query string.</param>
    /// <param name="body">The raw request body, if any.</param>
    /// <returns>The base64-encoded signature.</returns>
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
