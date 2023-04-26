using CRUDoperations.Filters.ActionFilters;
using CRUDoperations.StartupExtensions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServicesContracts;
using XServices;
using CRUDoperations.Middleware;

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

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

//Add extra log after request is completed
app.UseSerilogRequestLogging(); 

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandlingMiddleware();
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

app.UseAuthentication();  //Read Identity cookie  
app.UseRouting(); //Identify action metthod based on route
app.MapControllers(); // Execute the filter pipeline

app.Run();

//Make the auto-generated program accessible programmatically
public partial class Program { }