# Coinbase.AdvancedTrade Client

A lightweight .NET client for Coinbase Advanced Trade with clean DI, per-request credential resolution, and testability.

## Registering the client

```csharp
// Using Microsoft.Extensions.DependencyInjection
services
    .AddCoinbaseAdvancedTradeClient(options =>
    {
        // Package behavior only (no secrets here)
        options.BaseUri = new Uri("https://api.coinbase.com/");
        // options.BrokerageApiPath, options.HttpClientTimeout also available
    })
    .UseOptionsCredentials(); // or .UseCredentialsProvider<MyProvider>()

// If using the options-based provider, bind CoinbaseCredentialsOptions
services.Configure<CoinbaseCredentialsOptions>(cfg =>
{
    cfg.ApiKey = Environment.GetEnvironmentVariable("COINBASE_API_KEY");
    cfg.ApiSecret = Environment.GetEnvironmentVariable("COINBASE_API_SECRET");
    cfg.Passphrase = Environment.GetEnvironmentVariable("COINBASE_API_PASSPHRASE");
});
```

## Credentials providers

- Options-based (default helper): `.UseOptionsCredentials()` reads `CoinbaseCredentialsOptions` via `IOptionsMonitor` and supports reloads.
- Custom provider: `.UseCredentialsProvider<T>()` to plug in your own `ICoinbaseCredentialsProvider` (e.g., per-user/tenant source).

```csharp
public sealed class MyProvider : ICoinbaseCredentialsProvider
{
    public Task<CoinbaseCredentials> GetAsync(CancellationToken ct = default)
    {
        // Resolve from your user/tenant context
        return Task.FromResult(new CoinbaseCredentials(apiKey, apiSecretBase64, passphrase));
    }
}
```

## Fail-fast validation (hosted apps)

- The library registers a hosted service that runs at host startup and throws if no `ICoinbaseCredentialsProvider` is registered.
- This ensures misconfiguration is caught early.
- Non-hosted scenarios (plain ServiceCollection + BuildServiceProvider) don’t run hosted services; register a provider explicitly before resolving the client.

## Time and signing

- Uses `TimeProvider` (registered as `TimeProvider.System`) for timestamps; override with `FakeTimeProvider` in tests if needed.
- Request signing is stateless; `ICoinbaseRequestSigner.CreateSignature(string apiSecret, ...)` accepts the base64 secret from your provider.

## Usage

```csharp
var client = provider.GetRequiredService<ICoinbaseAdvancedTradeClient>();
var products = await client.Public.GetProductsAsync("SPOT");
var accounts = await client.Accounts.GetAccountsAsync(limit: 10);
```

## Testing

- Swap in a test credentials provider and `FakeTimeProvider` to write deterministic tests.
- The HTTP layer uses `IHttpClientFactory`; you can mock HTTP with libraries like `RichardSzalay.MockHttp` for unit tests.
