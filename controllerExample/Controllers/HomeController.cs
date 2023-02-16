using controllerExample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace controllerExample.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {

            return Content("<h1>Hello from home page</h1>", "text/plain");
            //return new ContentResult()
            //{
            //    Content = "Hello from home page",
            //    ContentType = "application/json"
            //};
        }

        [Route("person")]
        public IActionResult Person()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                FirstName = "Leandro",
                LastName = "Luz",
                Age = 25
            };

            //return new JsonResult(person);
            return Json(person);
        }

        [Route("file-download")]
        public IActionResult FileDonwload()
        {

            //VirtualFileResult for files contained in wwwroot folder
            //PhysicalFileResult for files contained outside file result
            //FileContentResult for byte files from bd etc...
            return new VirtualFileResult("/Requerimento.pdf", "application/pdf");
        }

        [Route("file-download2")]
        public IActionResult FileDonwload2()
        {
            return new PhysicalFileResult(@"D:\\Code\aspnetcore\\controllerExample\\wwwroot\\Requerimento.pdf", "application/pdf");
        }

        [Route("file-download3")]
        public IActionResult FileDonwload3()
        {
            var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            byte[] bytes = System.IO.File.ReadAllBytes(@"D:\\Code\aspnetcore\\controllerExample\\wwwroot\\Requerimento.pdf");
            //return new FileContentResult("Byte_array", "application/pdf");
            return File(bytes, "application/pdf");
        }

        [Route("book/{bookid?}/{isloggedin?}")]
        public IActionResult Book(int? bookid, [FromRoute] bool? isloggedin, Book book)
        {
            if(!bookid.HasValue)
            {
                return BadRequest("Book id shoud not be enty");
            }
            if (!Request.Query.ContainsKey("bookid"))
            {
                //Response.StatusCode = 400;
                //return Content("Book id is not supplied");
                return BadRequest();
            }

            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                Response.StatusCode = 400;
                return BadRequest("Book id shoud not be enty");
            }
            byte[] bytes = System.IO.File.ReadAllBytes(@"D:\\Code\aspnetcore\\controllerExample\\wwwroot\\Requerimento.pdf");

            return File(bytes, "application/pdf");
        }

        [Route("old")]
        public IActionResult OldRedirect()
        {
            //return new RedirectToActionResult(); 302
            //return RedirectToAction();

            //return RedirectToActionPermanent(); 301

            //Redirect from same domain
            //return LocalRedirect("/new"); 302 - Not Found
            //return LocalRedirectPermanent("/new"); //301

            // Redirect to another domain
            return RedirectPermanent("new"); // 301 - Redirect Permanently
            return Redirect("new"); // 302 - Not Found
        }

        [Route("new")]
        public IActionResult NewRedirect()
        {
            return Ok("you are redirected");
        }
    }
}