using Application.Common.Interfaces;
using Domain.Entities;
using FastEndpoints;
using FastEndpoints.Security;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    public static class Startup
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<FoodJournalContext>(
                options =>
                {
                    var connectionString = builder.Configuration["connectionString"];
                    if (string.IsNullOrEmpty(connectionString))
                        throw new InvalidOperationException("The connection string was not set");
                    var serverVersion = ServerVersion.AutoDetect(connectionString);
                    options.UseMySql(connectionString, serverVersion,
                        x =>
                        {
                            x.MigrationsAssembly("DiabetesFoodJournal.Data.EfCore");
                        });
                });
            builder.Services.AddFastEndpoints();
            builder.Services.AddAuthenticationJWTBearer("This is a super secret long key for authentication.");
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
        }
    }
}