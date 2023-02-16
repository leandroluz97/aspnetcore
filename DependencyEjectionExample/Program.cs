using Services;
using ServiceContracts;
using Autofac.Extensions.DependencyInjection;
using Autofac;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Services.Add(
//    new ServiceDescriptor(
//        typeof(ICitiesService),
//        typeof(CitiesService),
//        ServiceLifetime.Scoped
//        ));
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope(); //Scope
    //container.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency(); //Transient
    //container.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance(); //Singleton
});
//builder.Services.AddScoped<ICitiesService, CitiesService>();
//builder.Services.AddTransient<ICitiesService, CitiesService>();
//builder.Services.AddSingleton<ICitiesService, CitiesService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();


app.Run();
