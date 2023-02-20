using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers
{

    public class HomeController : Controller
    {
        private readonly WeatherApiOptions _options;
        //private readonly IConfiguration _configuration;
        //public HomeController(IConfiguration configuration)
        public HomeController(IOptions<WeatherApiOptions> weatherApiOptions)
        {
            _options = weatherApiOptions.Value;
        }
        [HttpGet("/")]
        public IActionResult Index()
        {
            //return Ok(_configuration["mykey"]);
            //return Ok(_configuration.GetValue<string>("mykey
            //WeatherApiOptions options = _configuration.GetSection("weatherapi").Get<WeatherApiOptions>();
            //var api = options.Api;
            //var apiKey = options.ApiKey;
            //ViewBag.Api_key = _configuration.GetSection("weatherApi")["apiKey"];
            //ViewBag.Api = _configuration["weatherApi:Api"];
            //ViewBag.MyKey = _configuration["mykey"];
            ViewBag.Api = _options.Api;
            ViewBag.MyKey = _options.ApiKey;
            return View(); 
        }
    }
}
