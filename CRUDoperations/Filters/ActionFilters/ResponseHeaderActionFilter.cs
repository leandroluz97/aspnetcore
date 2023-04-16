using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDoperations.Filters.ActionFilters
{
    public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private string Key;
        private string Value;
        private int Order;

        public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
        {
            Key = key;
            Value = value;
            Order = order;
        }
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
            filter.Key = Key;
            filter.Value = Value;
            filter.Order = Order;
            return filter;
        }
    }

    public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    //public class ResponseHeaderActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<ResponseHeaderActionFilter> _logger;
        public string Key { get; set; }
        public string Value { get; set; }     
        public int Order { get; set; }

        public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
        {
            _logger = logger;
        }



        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //_logger.LogInformation("{FilterName}.{MethodName} method - Before", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            await next(); //calls the subsequent filter or action method
            //_logger.LogInformation("{FilterName}.{MethodName} method - After", nameof(ResponseHeaderActionFilter), nameof(OnActionExecutionAsync));
            context.HttpContext.Response.Headers[Key] = Value;
        }
    }
}
