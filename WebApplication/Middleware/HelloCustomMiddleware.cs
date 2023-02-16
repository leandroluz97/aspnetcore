using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplicationExample.Middleware.HelloMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HelloCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloCustomMiddleware(RequestDelegate next)
        {
            //Before Logic
            _next = next;
            //After Logic
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var queries = httpContext.Request.Query;
            if(queries.ContainsKey("firstname") && queries.ContainsKey("lastname"))
            {
               await  httpContext.Response.WriteAsync(queries["firstname"] + " " + queries["lastname"]);
            }
            await  _next(httpContext);
            //await httpContext.Response.WriteAsync("returned");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HelloCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseHelloCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HelloCustomMiddleware>();
        }
    }
}
