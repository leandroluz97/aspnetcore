using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace Weather_app.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        private readonly IWeatherService _weatherService;

        public HomeController(IWeatherService weatherService)
        {
            _weatherService = weatherService;   
        }

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            var citiesWeather = _weatherService.GetCitiesWeather();
            if(citiesWeather == null)
            {
                return NotFound();
            }
            return Json(citiesWeather);
        }

        [Route("/{cityCode}")]
        [HttpGet]
        public IActionResult GetCityWeather([FromRoute] string cityCode)
        {
            var cityWeather = _weatherService.GetCityWeather(cityCode);
            if(cityWeather == null)
            {
                return NotFound();
            }
            return Json(cityWeather);
        }


    }
}
