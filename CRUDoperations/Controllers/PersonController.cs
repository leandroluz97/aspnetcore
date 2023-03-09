using Microsoft.AspNetCore.Mvc;

namespace CRUDoperations.Controllers
{
    public class PersonController : Controller
    {
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
