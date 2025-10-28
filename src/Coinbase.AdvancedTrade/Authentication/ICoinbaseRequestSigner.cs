namespace Coinbase.AdvancedTrade.Authentication;

/// <summary>
/// Produces Coinbase Advanced Trade request signatures.
/// </summary>
internal interface ICoinbaseRequestSigner
{
    string CreateSignature(DateTimeOffset timestamp, HttpMethod method, string requestPath, string? body);
}
