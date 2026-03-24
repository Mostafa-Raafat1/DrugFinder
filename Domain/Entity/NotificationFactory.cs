using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Factory
{
    public static class NotificationFactory
    {
        public static Notification ForPatient(int PatientId, string Message)
        {
            var notification = new Notification();
            notification.ForPaient(PatientId, Message);
            return notification;
        }
        public static Notification ForPharmacy(int PharmacyId, string Message)
        {
            var notification = new Notification();
            notification.ForPharmacy(PharmacyId, Message);
            return notification;
        }
    }
}
