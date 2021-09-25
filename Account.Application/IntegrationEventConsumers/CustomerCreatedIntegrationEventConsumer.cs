using Common.IntegrationEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Account.Application
{
    public class CustomerCreatedIntegrationEventConsumer : IConsumer<CustomerCreatedIntegrationEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public CustomerCreatedIntegrationEventConsumer(
            IMediator mediator,
            ILogger<CustomerCreatedIntegrationEventConsumer> logger
        )
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CustomerCreatedIntegrationEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(
                "-----{EventType} : ({@Event})",
                message.GetType(),
                message);


            await _mediator.Send(new CreateAccountCommand(
                message.CustomerId, message.CustomerName, message.CustomerEmail, message.InitialCredit)
            );

            await Task.CompletedTask;
        }
    }
}