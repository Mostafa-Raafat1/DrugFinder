using Application.Common;
using Application.Identity;
using Application.JWT;
using Application.Services;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService tokenService;
        private readonly IUserService userService;

        public AccountService(ITokenService tokenService, IUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }
        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            // Check the user's credentials using the IUserService
            var Result = await userService.CheckLoginCredentialsAsync(email, password);
            if (!Result.IsSuccess)
            {
                return Result<string>.Failure(Result.Error);
            }

            // If the credentials are valid, generate a JWT token using the ITokenService
            var roles = await userService.GetUserRolesAsync(email); // Get user roles Asynchronously for token generation
            var token = tokenService.GenerateToken(Result.Value, email, roles );

            return Result<string>.Success(token);

        }
    }
}
