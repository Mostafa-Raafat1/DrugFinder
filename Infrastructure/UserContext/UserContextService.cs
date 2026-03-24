using Application.UserContext;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserContext
{
    // Thisd service is responsible for providing information about the currently authenticated user, such as their ID, email, and roles. It uses the IHttpContextAccessor to access the current HTTP context and retrieve the user's claims.
    public class UserContextService : IUserContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string UserId => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

        public List<string> Roles => httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
    }
}
