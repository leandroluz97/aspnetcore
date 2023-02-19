using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddControllers();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
