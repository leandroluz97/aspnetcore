var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<HomeController>(); add specific controller
builder.Services.AddControllers(); // Add all the controllers with "Controller" suffix classe as services
var app = builder.Build();

app.UseStaticFiles();

app.MapControllers(); // replace both UseRouting and UseEnpoints

//app.UseRouting();
//app.UseEndpoints(endpoints => { 
//    endpoints.MapControllers(); //map/use all the controllers
//});

app.Run();
