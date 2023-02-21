using Microsoft.AspNetCore.Mvc;

namespace SocialMedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("/")]
        public IActionResult Index()
        {
            ViewBag.Facebook = _configuration["SocialMediaLinks:Facebook"];
            ViewBag.Instagram = _configuration["SocialMediaLinks:Instagram"];
            ViewBag.Twitter = _configuration["SocialMediaLinks:Twitter"];
            ViewBag.Youtube = _configuration["SocialMediaLinks:Youtube"];

            return View();
        }
    }
}
