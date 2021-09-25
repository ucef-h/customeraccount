using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Customer.Application
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;
        private readonly ICustomerIntegrationEventService _enquiriesIntegrationEvent;

        public TransactionBehaviour(

            ILogger<TransactionBehaviour<TRequest, TResponse>> logger,
            ICustomerIntegrationEventService enquiriesIntegrationEvent
          )
        {
            _enquiriesIntegrationEvent = enquiriesIntegrationEvent;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();

            await _enquiriesIntegrationEvent.PublishEventsThroughEventBusAsync();

            return response;
        }
    }
}
