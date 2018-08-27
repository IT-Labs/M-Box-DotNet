using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using ItLabs.MBox.Contracts.Entities;
using Microsoft.AspNetCore.Identity;
using ItLabs.MBox.Data;
using Microsoft.Extensions.Configuration;

namespace ItLabs.MBox.Domain
{
    public class SetupServices
    {
        public static void AddEntityFrameworkServices(IServiceCollection services)
        {
            var conncetionString = "Server=localhost; Port=5432; Database=ItLabs.MBoxApplication; User ID=postgres; Password=";

            services.AddEntityFrameworkNpgsql().AddDbContext<MBoxDbContext>(opt =>
                opt.UseNpgsql(conncetionString, b=>b.MigrationsAssembly("ItLabs.MBox.Data")));

            /*services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MBoxDbContext>()
                .AddDefaultTokenProviders();*/

            services.AddIdentity<ApplicationUser, ApplicationUserRole>()
                .AddEntityFrameworkStores<MBoxDbContext>()
                .AddDefaultTokenProviders();
         
        }
    }
}
