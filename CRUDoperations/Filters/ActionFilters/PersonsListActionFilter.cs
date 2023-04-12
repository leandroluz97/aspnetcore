﻿using Microsoft.AspNetCore.Mvc.Filters;
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
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
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
