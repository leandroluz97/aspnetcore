using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDoperations.Filters.ResultFilters
{
    public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // skip all the other filters where [SkipFilter] is used
            if (context.Filters.OfType<SkipFilter>().Any())
            {
                return;
            }
        }
    }
}
