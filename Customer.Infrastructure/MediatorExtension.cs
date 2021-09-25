using Core;
using MediatR;
using System.Threading.Tasks;

namespace Customer.Infrastructure
{
    static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, BaseEntity entity)
        {
            var domainEvents = entity.DomainEvents;

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent);
            }

            entity.ArchiveDomainEvents();
        }
    }
}
