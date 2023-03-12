using Microsoft.AspNetCore.Mvc;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace CRUDoperations.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personsService;
        private readonly ICountriesService _countriesService;   
        public PersonController(IPersonService personService, ICountriesService  countriesService)
        {
            _personsService = personService;
            _countriesService = countriesService;
        }
        

        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchText, string sortBy = "PersonName", SortOrderOptions sortOrder = SortOrderOptions.ASC )
        {
            //Searching
            ViewBag.SearchFields = new Dictionary<string, string>()
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
            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchText);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchText = searchText;

            //Sorting
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            var countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("persons/create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                var countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;   
                ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage).ToList();
                return View();
            }

            _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");
        }
    }
}
