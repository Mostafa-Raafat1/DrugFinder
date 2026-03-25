using Application.Services;
using Domain.DomainEvent.Events;
using Infrastructure.Repos.Interfaces;
using Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Event.Handlers
{
    public class NotifyNearByPharmaciesHandler : IEventHandler<DrugRequestedCreatedEvent>
    {
        private readonly INotificationService notificationService;
        private readonly IUnitOfWork uow;

        public NotifyNearByPharmaciesHandler(INotificationService notificationService, IUnitOfWork Uow)
        {
            this.notificationService = notificationService;
            uow = Uow;
        }
        public async Task Handle(DrugRequestedCreatedEvent domainEvent)
        {
            // Get the drug request details and patient information to include in the notification message
            var Request = await uow.DrugRequest.getDrugRequestByDomainId(domainEvent.RequestId);
            var patient = await  uow.Patient.getPatientByDomainId(domainEvent.PatientId);
            string Message = $"A new drug request has been created for drug ID: {Request.Id}, Patient Name: {patient.FName} {patient.SName}. Please check the request details and respond accordingly.";
            // Get Nearby Pharmacies and send them the notification

            await notificationService.SendNotificationToNearbyPharmaciesAsync(patient.Location, Message);
        }
    }
}
