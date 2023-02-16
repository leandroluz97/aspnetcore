


namespace WebApplicationExample.Middleware.CustomMiddleware
{
    public class MycustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
           await context.Response.WriteAsync("First Middleware");
           await next(context);
           await context.Response.WriteAsync("The first same middleware");
        }
    }

    static class CustomMiddlewareExtension
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MycustomMiddleware>();
        }
    }
}
