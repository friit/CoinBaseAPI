using Coinbase.AdvancedTrade.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Coinbase.AdvancedTrade.DependencyInjection;

internal sealed class CoinbaseSetupValidationHostedService : IHostedService
{
    private readonly IServiceProviderIsService _isService;

    public CoinbaseSetupValidationHostedService(IServiceProviderIsService isService)
    {
        _isService = isService ?? throw new ArgumentNullException(nameof(isService));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_isService.IsService(typeof(ICoinbaseCredentialsProvider)))
        {
            throw new InvalidOperationException("ICoinbaseCredentialsProvider is not registered. Configure credentials via UseOptionsCredentials or UseCredentialsProvider.");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
