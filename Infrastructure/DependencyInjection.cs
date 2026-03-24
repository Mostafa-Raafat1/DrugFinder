using Application.Event.Dispatcher;
using Application.Event.Handlers;
using Application.Identity;
using Application.JWT;
using Application.Services;
using Application.UserContext;
using Domain.DomainEvent.Events;
using Infrastructure.Identity;
using Infrastructure.Persistence.DbContext;
using Infrastructure.Repos.Implementations;
using Infrastructure.Repos.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Identity;
using Infrastructure.Services.JWT;
using Infrastructure.UnitOfWork;
using Infrastructure.UserContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            // Add JWT Authentication
            var jwtSettings = configuration.GetSection("Jwt");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"])
                    ),

                    RoleClaimType = ClaimTypes.Role
                };
            });

            // Add Database Context

            services.AddDbContextPool<AppDbContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("con"),
                   b => b.MigrationsAssembly("Infrastructure")
                   .UseNetTopologySuite()));

            // Repositories
            services.AddScoped<IPatient, PatientRepo>();
            services.AddScoped<IPharmacy, PharmacyRepo>();
            services.AddScoped<IDrugRequest, DrugRequestRepo>();
            services.AddScoped<INotification, NotificationRepo>();
            services.AddScoped<IPharamcyResponse, PharmacyResponseRepo>();
            services.AddScoped<IUnitOfWork, Infrastructure.UnitOfWork.UnitOfWork>();
            // Handlers
            services.AddScoped<IEventHandler<DrugRequestedCreatedEvent>, NotifyNearByPharmaciesHandler>();
            //Dispatcher
            services.AddScoped<IDomainEventDispatcher, DomainEventDisptcher>();

            //services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDrugRequestService, DrugRequestService>();
            services.AddScoped<IPharmacyService, PharmacyService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserContext, UserContextService>();

            return services;
        }
    }
}
