using CRUDoperations.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServicesContracts;
using XServices;

namespace CRUDoperations.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ResponseHeaderActionFilter>();

            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add<ResponseHeaderActionFilter>();
                //services store the services
                //services.BuildServiceProvider() dispatchs the services
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                options.Filters.Add(new ResponseHeaderActionFilter(logger) { Key = "My-Key-From-Global", Value = "My-Value-From-Global", Order = 2 });
            });

            //Add services into IoC container
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddDbContext<ApplicationDbContext>
                (options =>
                {
                    options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
                });

            //Configure Identity
            services
                .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 3;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            //Enforces authorization policy (user must be authenticated) for all the user methods
            services.AddAuthorization(options => {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            //Page to redirect user if not login
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });
                
            //Add custom properties to logging
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
                | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
