using Application.Event.Dispatcher;
using Domain.Entity;
using Infrastructure.Persistence.DbContext;
using Infrastructure.Repos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;
        private readonly IDomainEventDispatcher dispatcher;

        public UnitOfWork(AppDbContext dbContext,
            IPatient patientRepo,
            IPharmacy pharmacyRepo,
            IDrugRequest drugRequestRepo,
            INotification notificationRepo,
            IPharamcyResponse pharmacyResponseRepo,
            IDomainEventDispatcher dispatcher
            )
        {
            this.dbContext = dbContext;

            Patient = patientRepo;
            Pharmacy = pharmacyRepo;
            DrugRequest = drugRequestRepo;
            Notification = notificationRepo;
            PharamcyResponse = pharmacyResponseRepo;
            this.dispatcher = dispatcher;
        }

        public IPatient Patient { get; }
        public IPharmacy Pharmacy { get; }
        public IDrugRequest DrugRequest { get; }
        public INotification Notification { get; }
        public IPharamcyResponse PharamcyResponse { get; }

        public async Task<int> SaveChangesAsync()
        {
            // Save entities first
            var result = await dbContext.SaveChangesAsync();

            // Find entities with domain events
            var entitiesWithEvents = dbContext.ChangeTracker
                .Entries<Entity_>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            // Dispatch events and clear them
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents(); // important!

                foreach (var domainEvent in events)
                {
                    await dispatcher.DispatchAsync((dynamic)domainEvent);
                }
            }

            return result;
        }
    }
}
