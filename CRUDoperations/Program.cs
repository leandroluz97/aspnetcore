using ServicesContracts;
using XServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//Add services into IoC container
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IPersonService, PersonService>();

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
