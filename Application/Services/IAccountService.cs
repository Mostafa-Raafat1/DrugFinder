using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<Result<string>> LoginAsync(string email, string password);
    }
}
