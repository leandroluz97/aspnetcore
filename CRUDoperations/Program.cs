using CRUDoperations.Filters.ActionFilters;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServicesContracts;
using XServices;

var builder = WebApplication.CreateBuilder(args);

//Logging
//builder.Host.ConfigureLogging(loggingProvider => { 
//    loggingProvider.ClearProviders(); //Cleared all providers
//    loggingProvider.AddConsole(); //Added console logger
//    loggingProvider.AddDebug(); //Added console debug
//    loggingProvider.AddEventLog(); //Added console debug
//});

//Serilog
builder.Host.UseSerilog((
    HostBuilderContext context, 
    IServiceProvider services, 
    LoggerConfiguration configuration) => 
    {
        configuration.ReadFrom.Configuration(context.Configuration) //Read configuration settings from build-in Iconfiguration
        .ReadFrom.Services(services) //Read out current app's services
        .Enrich.FromLogContext()
        .WriteTo.Console();
    }
);

builder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add<ResponseHeaderActionFilter>();
    //builder.Services store the services
    //builder.Services.BuildServiceProvider() dispatchs the services
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
    options.Filters.Add(new ResponseHeaderActionFilter(logger, "My-Key-From-Global", "My-Value-From-Global", 2));
});

//Add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddDbContext<ApplicationDbContext>
    (options =>
    {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    });


//Add custom properties to logging
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties
    | Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});

var app = builder.Build();

//Add extra log after request is completed
app.UseSerilogRequestLogging(); 

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//To display loggs related to request same as in nodejs
app.UseHttpLogging();

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

if (!builder.Environment.IsEnvironment("Test"))
{
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

//Make the auto-generated program accessible programmatically
public partial class Program { }