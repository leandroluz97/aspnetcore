using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTO;

namespace CRUDoperations.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDto registerDto)
        {
            return RedirectToAction(nameof(PersonController.Index), "Person");
        }
    }
}
