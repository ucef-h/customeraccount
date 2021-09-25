using Core;
using System.Threading.Tasks;

namespace Customer.Application
{
    public interface ICustomerIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync();
        Task AddAndSaveEventAsync(IntegrationEvent @event);
    }
}
