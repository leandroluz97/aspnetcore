using CRUDoperations.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServicesContracts.DTO;

namespace CRUDoperations.Filters.ActionFilters
{
    public class PersonsListActionFilter : IActionFilter
    {
        private readonly ILogger<PersonsListActionFilter> _logger;

        public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuted method");

            PersonController personController = (PersonController)context.Controller;
            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if(parameters is not null)
            {
                if (parameters.ContainsKey("searchBy"))
                    personController.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);

                if (parameters.ContainsKey("searchText"))
                    personController.ViewData["currentCurrentSearchText"] = Convert.ToString(parameters["searchText"]);

                if (parameters.ContainsKey("sortBy"))
                    personController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);

                if (parameters.ContainsKey("sortOrder"))
                    personController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["sortOrder"]);
            }
            personController.ViewData["SearchFields"] = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.CountryId), "Country" },
                {nameof(PersonResponse.Address), "Address" },
                {nameof(PersonResponse.Age), "Age" },
                {nameof(PersonResponse.ReceiveNewsLetters), "ReceiveNewsLetters" },
            };
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            _logger.LogInformation("PersonsListActionFilter.OnActionExecuting method");
            if (context.ActionArguments.ContainsKey("searchBy"))
            {
                string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

                if (!string.IsNullOrWhiteSpace(searchBy))
                {
                    var searchByOptions = new List<string>() 
                    { 
                        nameof(PersonResponse.PersonName), 
                        nameof(PersonResponse.Email), 
                        nameof(PersonResponse.DateOfBirth), 
                        nameof(PersonResponse.Gender), 
                        nameof(PersonResponse.CountryId), 
                        nameof(PersonResponse.Address), 
                    };

                    if (searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy updated value {searchBy}", searchBy);
                    }
                }
            }
        }
    }
}
