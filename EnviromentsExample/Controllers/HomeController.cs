using Microsoft.AspNetCore.Mvc;

namespace EnviromentsExample.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment _webHostEnv;
        public HomeController(IWebHostEnvironment webHostEnv)
        {
            _webHostEnv = webHostEnv;
        }

        [Route("/")]
        public IActionResult Index()
        {
            //_webHostEnv.IsDevelopment();
            ViewBag.CurrentEnviroment = _webHostEnv.EnvironmentName;
            return View();
        }
    }
}
