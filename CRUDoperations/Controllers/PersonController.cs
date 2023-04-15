using CRUDoperations.Filters.ActionFilters;
using CRUDoperations.Filters.ResourceFilters;
using CRUDoperations.Filters.ResultFilters;
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
        private readonly ILogger<PersonController> _logger;
        public PersonController(IPersonService personService, ICountriesService countriesService, ILogger<PersonController> logger)
        {
            _personsService = personService;
            _countriesService = countriesService;
            _logger = logger;   
        }


        [Route("persons/index")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key", "Custom-Value", 1 })]
        [TypeFilter(typeof(PersonListResultFilter))]
        public async Task<IActionResult> Index(string searchBy, string? searchText, string sortBy = "PersonName", SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogInformation($"searchBy: {searchBy}, searchtext: {searchText}, sortBy: {sortBy}, sortOrder: {sortOrder}");
            //Searching
            //ViewBag.SearchFields = new Dictionary<string, string>()
            //{
            //    {nameof(PersonResponse.PersonName), "Person Name" },
            //    {nameof(PersonResponse.Email), "Email" },
            //    {nameof(PersonResponse.DateOfBirth), "Date of Birth" },
            //    {nameof(PersonResponse.Gender), "Gender" },
            //    {nameof(PersonResponse.CountryId), "Country" },
            //    {nameof(PersonResponse.Address), "Address" },
            //    {nameof(PersonResponse.Age), "Age" },
            //    {nameof(PersonResponse.ReceiveNewsLetters), "ReceiveNewsLetters" },
            //};
            List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchText);
            //ViewBag.CurrentSearchBy = searchBy;
            //ViewBag.CurrentSearchText = searchText;

            //Sorting
            List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            return View(sortedPersons);
        }

        [Route("persons/create")]
        [HttpGet]
        [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "my-Key", "my-Value", 4 })]
        public async Task<IActionResult> Create()
        {
            var countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("persons/create")]
        [HttpPost]
        [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
        //[TypeFilter(typeof(FeatureDisabledResourceFilter))]
        public async Task<IActionResult>  Create(PersonAddRequest personRequest)
        {
            //if (!ModelState.IsValid)
            //{
            //    var countries = _countriesService.GetAllCountries();
            //    ViewBag.Countries = countries;
            //    ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage).ToList();
            //    return View(personRequest);
            //}

            await _personsService.AddPerson(personRequest);
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
