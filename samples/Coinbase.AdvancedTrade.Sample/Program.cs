using Coinbase.AdvancedTrade.Clients.Core;
using Coinbase.AdvancedTrade.DependencyInjection;
using Coinbase.AdvancedTrade.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = SampleOptions.FromEnvironment();
// For the sample, require credentials env vars to be present.
if (!configuration.IsConfigured)
{
    Console.WriteLine("Configure COINBASE_API_KEY, COINBASE_API_SECRET, and COINBASE_API_PASSPHRASE environment variables before running the sample.");
    return;
}

var services = new ServiceCollection();
services.AddLogging(logging => logging.AddSimpleConsole(options => options.SingleLine = true));
services.AddCoinbaseAdvancedTradeClient(options =>
{
    options.BaseUri = configuration.BaseUri;
})
// Configure credentials via options and enable the options-based provider
.UseOptionsCredentials();

// Bind credentials options directly for the sample
services.Configure<CoinbaseCredentialsOptions>(opts =>
{
    opts.ApiKey = configuration.ApiKey;
    opts.ApiSecret = configuration.ApiSecret;
    opts.Passphrase = configuration.Passphrase;
});

using var provider = services.BuildServiceProvider();
var logger = provider.GetRequiredService<ILoggerFactory>().CreateLogger("Sample");

var client = provider.GetRequiredService<ICoinbaseAdvancedTradeClient>();

logger.LogInformation("Fetching public products...");
var products = await client.Public.GetProductsAsync("SPOT");
var firstProduct = products.Products.FirstOrDefault();
if (firstProduct is not null)
{
    logger.LogInformation("First product: {ProductId} ({DisplayName})", firstProduct.ProductId, firstProduct.DisplayName);
}
else
{
    logger.LogWarning("No products returned.");
}

logger.LogInformation("Listing first account...");
var accounts = await client.Accounts.GetAccountsAsync(limit: 1);
if (accounts.Accounts.Count > 0)
{
    var account = accounts.Accounts[0];
    logger.LogInformation("Account {Name} {Currency} available {Balance}", account.Name, account.Currency, account.AvailableBalance.Value);
}
else
{
    logger.LogWarning("No accounts available.");
}

sealed class SampleOptions
{
    public string ApiKey { get; init; } = string.Empty;
    public string ApiSecret { get; init; } = string.Empty;
    public string Passphrase { get; init; } = string.Empty;
    public Uri BaseUri { get; init; } = new("https://api.coinbase.com/");
    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(ApiSecret) && !string.IsNullOrWhiteSpace(Passphrase);

    public static SampleOptions FromEnvironment() => new()
    {
        ApiKey = Environment.GetEnvironmentVariable("COINBASE_API_KEY") ?? string.Empty,
        ApiSecret = Environment.GetEnvironmentVariable("COINBASE_API_SECRET") ?? string.Empty,
        Passphrase = Environment.GetEnvironmentVariable("COINBASE_API_PASSPHRASE") ?? string.Empty
    };
}
