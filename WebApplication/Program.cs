using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using WebApplicationExample.Middleware.CustomMiddleware;
using WebApplicationExample.Middleware.HelloMiddleware;
using WebApplicationExample.Routing;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "myroot" //change wwwroot default to myroot
});

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("months", typeof(CustomConstrains));
});
builder.Services.AddTransient<MycustomMiddleware>();
var app = builder.Build();

app.UseStaticFiles(); // works with  root path (default)
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "mywebroot")
        )
}); // works with "mywebroot"
#region ROUTING

//enabled routing
app.UseRouting();

//creating endpoints
app.UseEndpoints(endpoints => {
    //add your end points

    endpoints.Map("/files/{filename}.{extension}", async (context) =>
    {
        var filename = Convert.ToString(context.Request.RouteValues["filename"]);
        var extension = Convert.ToString(context.Request.RouteValues["extension"]);
        await context.Response.WriteAsync($"In files {filename} - {extension}");
    });

    endpoints.Map("/employee/profile/{EmployeeName=leandro}", async (context) =>
    {
        var employeeName = Convert.ToString(context.Request.RouteValues["employeeName"]);
        await context.Response.WriteAsync($"employee {employeeName}");
    });

    endpoints.Map("/product/details/{Id=1}", async (context) =>
    {
        var id = Convert.ToInt32(context.Request.RouteValues["id"]);
        await context.Response.WriteAsync($"Details ID: {id}");
    });

    endpoints.Map("/daily-digest-report/{reportdate:datetime}", async (context) =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"Daily Digest report {reportDate.ToShortDateString()}");
    });

    endpoints.Map("/cities/{city:guid}", async (context) =>
    {
        Guid.TryParse(Convert.ToString(context.Request.RouteValues["city"]), out Guid city);
        await context.Response.WriteAsync($"Daily Digest report {city}");
    });

    ///sales-report/{year:int:min(1900)}/{months:regex(^(apr|jul|oct|jan)$)}
    endpoints.Map("/sales-report/{year:int:min(1900)}/{months:months}", async (context) =>
    {
        Guid.TryParse(Convert.ToString(context.Request.RouteValues["city"]), out Guid city);
        await context.Response.WriteAsync($"Daily Digest report {city}");
    });

    endpoints.Map("/sales-report/2023/jan", async (context) => {
        await context.Response.WriteAsync(""); 
    });

    endpoints.Map("/map1", async (context) =>
    {
        await context.Response.WriteAsync("In Map 1");
    });

    endpoints.Map("map2", async (context) =>
    {
        await context.Response.WriteAsync("In Map 2");
    });
    
    endpoints.Map("map3", async (context) =>
    {
        await context.Response.WriteAsync("In Map 3");
    });
    
});

app.Run(async ( HttpContext context) => {
    await context.Response.WriteAsync("Index");
});

#endregion

#region  MIDDLEWARE
//app.MapGet("/", () => "Hello World!");

//Middleware 1
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await context.Response.WriteAsync("Hello world   -");
//    await next(context);
//});

//Middleware 2
//app.UseMiddleware<MycustomMiddleware>();
//app.UseMyCustomMiddleware(); 
//app.UseHelloCustomMiddleware();

//app.Run(async (HttpContext context) =>
//{
//    await context.Response.WriteAsync("last one");
//});


//app.Run(async (HttpContext context) =>
// {

//StreamReader reader = new StreamReader(context.Request.Body);
//var body = await reader.ReadToEndAsync();
//await context.Response.WriteAsJsonAsync(JsonSerializer.Deserialize<Person>(body));


//StreamReader reader = new StreamReader(context.Request.Body)
//var body = await reader.ReadToEndAsync();
//Dictionary<string, StringValues> queryDic = QueryHelpers.ParseQuery(body);

//if (queryDic.ContainsKey("firstName"))
//{
//    string firstName = queryDic["firstName"];
//    await context.Response.WriteAsync($"<h1>{firstName}</h1>");
//}


//context.Response.ContentType = "text/html";
//if (context.Request.Method == "GET")
//{
//   if (context.Request.Query.ContainsKey("id"))
//    {
//        var id = context.Request.Query["id"];
//        await context.Response.WriteAsync($"<h1>{id}</h1>");
//    }
//}

//string path =  context.Request.Path;
//var queries = context.Request.Query;
//var query = queries.First();
//context.Response.Headers["MyKey"] = "my value";
//context.Response.StatusCode = 200;
//context.Response.ContentType = "text/html";
//context.Response.ContentType = "application/json";


//});

//app.UseWhen(
//        context => context.Request.Query.ContainsKey("username"),
//        app =>
//        {
//            app.Use(async (context, next) =>
//            {
//                await context.Response.WriteAsync("hello from Middleware branch");
//                await next(context);
//            });
//        }
//    );

//app.MapPost("/user", async (HttpContext context) => {

//    StreamReader reader = new StreamReader(context.Request.Body);
//    var body = await reader.ReadToEndAsync();
//    await context.Response.WriteAsJsonAsync(JsonSerializer.Deserialize<object>(body));
//});
#endregion

app.Run();

