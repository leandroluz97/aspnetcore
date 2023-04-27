using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesContracts.DTO;

namespace CRUDoperations.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInMAnager)
        {
            _userManager = userManager;
            _signInManager = signInMAnager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage);
                return View(registerDto);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone,
                UserName = registerDto.Email,
                PersonName = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction(nameof(PersonController.Index), "Person");
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }
            return View(registerDto);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(err => err.Errors).Select(err => err.ErrorMessage);
                return View(loginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent:false, lockoutOnFailure:false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(PersonController.Index), "Person");
            }
            ModelState.AddModelError("Login","Invalid email or password");
            return View(loginDto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(PersonController.Index), "Person");
        }
    }
}
