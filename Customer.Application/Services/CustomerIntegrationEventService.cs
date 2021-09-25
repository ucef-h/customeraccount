using Core;
using IntegrationLog.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Customer.Application
{
    public class CustomerIntegrationEventService : ICustomerIntegrationEventService
    {
        private readonly ILogger<CustomerIntegrationEventService> _logger;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly IBusControl _endPoint;
        private readonly RequestTransient _requestTransient;

        public CustomerIntegrationEventService(
            ILogger<CustomerIntegrationEventService> logger,
            IIntegrationEventLogService eventLogService,
            IBusControl endPoint,
            RequestTransient requestTransient
        )
        {
            _logger = logger;
            _eventLogService = eventLogService;
            _endPoint = endPoint;
            _requestTransient = requestTransient;
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent @event)
        {
            _logger.LogInformation(
                "----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", @event.Id,
                @event);

            await _eventLogService.SaveEventAsync(@event, _requestTransient.TransactionId);
        }

        public async Task PublishEventsThroughEventBusAsync()
        {
            var pendingLogEvents =
                await _eventLogService.RetrieveEventLogsPendingToPublishAsync(_requestTransient.TransactionId);

            foreach (var logEvent in pendingLogEvents)
            {
                _logger.LogInformation(
                    "----- Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvent.Id,
                    logEvent.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvent.Id);
                    await _endPoint.Publish(logEvent.IntegrationEvent, logEvent.IntegrationEvent.GetType());
                    await _eventLogService.MarkEventAsPublishedAsync(logEvent.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId}", logEvent.Id);

                    await _eventLogService.MarkEventAsFailedAsync(logEvent.Id);
                }
            }
        }
    }
}