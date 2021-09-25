using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Bus.DependencyInjection
{
    public class MassTransitHostedService : IHostedService
    {
        private readonly IBusControl _depot;

        public MassTransitHostedService(IBusControl depot)
        {
            _depot = depot;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _depot.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _depot.StopAsync(cancellationToken);
        }
    }
}