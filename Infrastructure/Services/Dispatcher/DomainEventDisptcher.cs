using Application.Event.Handlers;
using Domain.DomainEvent;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event.Dispatcher
{
    public class DomainEventDisptcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider serviceProvider;

        public DomainEventDisptcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public async Task DispatchAsync<T>(T domainEvent) where T : IDomainEvent
        {
            // Resolve All hadnlers for this event
            var handlers = serviceProvider.GetServices<IEventHandler<T>>();

            foreach(var handler in handlers)
            {
                await handler.Handle(domainEvent);
            }
        }
    }
}
