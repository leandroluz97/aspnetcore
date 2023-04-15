using CRUDoperations.Controllers;
using CRUDoperations.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
using ServicesContracts;

namespace CRUDoperations.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;
        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Todo : Before logic
            if(context.Controller is PersonController personsController)
            {
                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesService.GetAllCountries();
                    personsController.ViewBag.Countries = countries;
                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result = personsController.View(personRequest);  //short-circuits or skips the subsequent action filter & action method
                }
            }
            else
            {
                await next();
            }
            //Todo : After logic
        }
    }
}
