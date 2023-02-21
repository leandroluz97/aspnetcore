//* Used for development
//dotnet user-secrets init
//dotnet user-secrets set "weatherapi:ClientID" "ClientID from client secrets"
//* Used for production
//Used for development
//$Env: weatherapi__ClientID = "Client from environment variables"
using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//Supply an object of weatherApiOption  as a Service
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherApi"));
// Load MyOwnConfigFile.json
builder.Host.ConfigureAppConfiguration((hostingContext, config) => 
{
    config.AddJsonFile("MuOwnConfigFile", optional:true, reloadOnChange:true );
});


var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();   
app.MapControllers();


app.Run();
