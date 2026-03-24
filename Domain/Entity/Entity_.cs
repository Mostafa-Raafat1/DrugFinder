using Domain.DomainEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public abstract class Entity_
    {
        
        public Guid Id { get; protected set; } // Domain Id
        public int DBId { get; protected set; } // Database Id

        private List<IDomainEvent> domainEvents = new(); // Field to hold domain events
        public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly(); // Expose domain events as read-only collection

        public override bool Equals(object? obj)
        {
            if(obj is not Entity_ other)
                return false;

            if(ReferenceEquals(this, other))
                return true;

            if (obj == null || obj.GetType() != GetType())
                return false;

            return Id == other.Id;
        }

        public static bool operator ==(Entity_? left, Entity_? right)
        {
            // Handle null cases
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;

            return left.Equals(right);
        }
        public static bool operator !=(Entity_? left, Entity_? right)
        {
            // oppose te == operator
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        // Method to add a domain event to the entity
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }

        // Method to clear domain events after they have been dispatched
        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }
    }
}
