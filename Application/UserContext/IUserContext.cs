using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserContext
{
    public interface IUserContext
    {
        string UserId { get; }
        string email { get; }
        List<string> Roles { get; }
    }
}
