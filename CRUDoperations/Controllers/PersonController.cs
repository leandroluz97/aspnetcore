using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;

namespace CRUDoperations.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personsService;
        public PersonController(IPersonService personService)
        {
            _personsService = personService;
        }
        

        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchText)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name" },
                {nameof(PersonResponse.Email), "Email" },
                {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                {nameof(PersonResponse.Gender), "Gender" },
                {nameof(PersonResponse.CountryId), "Country" },
                {nameof(PersonResponse.Address), "Address" },
            };
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchText);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchText = searchText;
            return View(persons);
        }
    }
}
