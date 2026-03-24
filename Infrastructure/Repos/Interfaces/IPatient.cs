using Domain.Domain;
using Infrastructure.Repos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repos.Interfaces
{
    public interface IPatient : IRepo<Patient>
    {
        Patient getPatientByUserAppId(string appUserId);
    }
}
