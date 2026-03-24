using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event.Dispatcher
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync<T>(T domainEvent) where T : Domain.DomainEvent.IDomainEvent;
    }
}
