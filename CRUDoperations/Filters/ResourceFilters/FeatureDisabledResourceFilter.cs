using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDoperations.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private readonly bool _disabled;

        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
        {  
            _logger = logger;
            _disabled = isDisabled;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} Before", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
            if (_disabled)
            {
                context.Result = new NotFoundResult();
            }
            else
            {
                await next();
                _logger.LogInformation("{FilterName}.{MethodName} After", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
            }
        }
    }
}
