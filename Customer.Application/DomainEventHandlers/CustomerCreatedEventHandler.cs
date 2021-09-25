using Common.IntegrationEvents;
using Customer.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.Application
{
    public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
    {
        private readonly ILogger _logger;
        private readonly ICustomerIntegrationEventService _customerIntegrationEvent;


        public CustomerCreatedEventHandler(
            ILogger<CustomerCreatedEventHandler> logger,
            ICustomerIntegrationEventService customerIntegrationEvent
        )
        {
            _logger = logger;
            _customerIntegrationEvent = customerIntegrationEvent;
        }

        public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
        {
            var customer = notification.Customer();
            _logger.LogInformation("----- CustomerCreatedEventEventHandler - CustomerId: {CustomerId}", customer.Id);

            if (customer.PositiveInitialCredit())
            {
                await _customerIntegrationEvent.AddAndSaveEventAsync(
                    new CustomerCreatedIntegrationEvent(
                        customer.Id, customer.CustomerName, customer.CustomerEmail, customer.InitialCredit)
                );
            }

            await Task.CompletedTask;
        }
    }
}