using TREASURY.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TREASURY.DomainObjects.Auth;
using TREASURY.Repository.Interface;
using TREASURY.Repository.Implement;

namespace TREASURY.Installers
{
    public class DbInstaller : IInstaller
    { 
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                   options.UseSqlServer(
                       configuration.GetConnectionString("DefaultConnection")));


            services.AddScoped<IAssetClassificationService, AssetClassificationService>();
            services.AddScoped<IAdditionService, AdditionService>();
            //services.AddScoped<IDisposalService, DisposalService>();
            services.AddScoped<IRegisterService, RegisterService>();
           // services.AddScoped<IReassessmentService, ReassessmentService>();

            services.AddDefaultIdentity<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUTREASURYrcase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddAutoMaTREASURYr(typeof(Startup)); 
            services.AddMediatR(typeof(Startup));
            services.AddMvc();  

            

        }
    }
}
