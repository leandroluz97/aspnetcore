using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
//Supply an object of weatherApiOption  as a Service
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("weatherApi"));
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();   
app.MapControllers();


app.Run();
