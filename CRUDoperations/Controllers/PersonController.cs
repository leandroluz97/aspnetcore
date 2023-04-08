using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;

namespace CRUDoperations.Controllers
{
    public class PersonController : Controller
    {
        private readonly IPersonService _personsService;
        private readonly ICountriesService _countriesService;
        public PersonController(IPersonService personService, ICountriesService countriesService)
        {
            _personsService = personService;
            _countriesService = countriesService;
        }


        [Route("persons/index")]
        [Route("/")]
        public async Task<IActionResult> Index(string searchBy, string? searchText, string sortBy = "PersonName", SortOrderOptions sortOrder = SortOrderOptions.ASC)
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
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchText);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchText = searchText;

            //Sorting
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        [Route("persons/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("persons/create")]
        [HttpPost]
        public async Task<IActionResult>  Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                var countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;
                ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage).ToList();
                return View(personAddRequest);
            }

            _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");
        }

        [Route("persons/PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            var persons = await _personsService.GetAllPersons();
            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins { Top = 20, Right = 20, Bottom = 20, Left = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("persons/PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("persons/PersonsExcel")]
        public async Task<IActionResult> PersonsEXCEL()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsCSV();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}
