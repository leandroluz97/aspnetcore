using Entities;
using Microsoft.EntityFrameworkCore;
using ServicesContracts;
using XServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//Add services into IoC container
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddDbContext<PersonsDbContext>
    (options =>
    {
        options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    });

var app = builder.Build();
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
