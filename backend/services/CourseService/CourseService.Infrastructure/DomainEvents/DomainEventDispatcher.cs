using CourseService.Domain;
using CourseService.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CourseService.Infrastructure.DomainEvents
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task Dispatch(DomainEvent domainEvent)
        {
            var handlers = _serviceProvider.GetKeyedServices<IDomainEventHandler>(domainEvent.GetType());

            if (!handlers.Any()) return;

            foreach (var handler in handlers)
            {
                try
                {
                    await handler.Handle(domainEvent);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error while handling domain event {domainEvent.GetType().Name}: {ex.Message}");
                }
            }
        }

        public async Task DispatchFrom(Entity entity)
        {
            foreach (var domainEvent in entity.DomainEvents)
            {
                await Dispatch(domainEvent);
            }

            entity.ClearDomainEvents();
        }
    }
}
