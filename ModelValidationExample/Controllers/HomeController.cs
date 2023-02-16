using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelValidationExample.CustomModelBinders;

namespace ModelValidationExample.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
        [Route("/Register")]
        //[Bind(nameof(Person.PersonName), nameof(Person.Password), ...)]
        //public IActionResult Index(Person person,, [FromHeader(Name ="User-Agent")] string UserAgent)
        //public IActionResult Index([FromBody][ModelBinder(BinderType = typeof(PersonModelBinder))] Person person)
        public IActionResult Index([FromBody] Person person)
        {
           
            if (!ModelState.IsValid)
            {
                List<string> errorList = ModelState.Values.SelectMany(value => value.Errors.Select(err => err.ErrorMessage)).ToList();
                string errors = string.Join("\n", errorList);
                return BadRequest(errors);
                //foreach (var value in ModelState.Values)
                //{
                //    foreach (var error in value.Errors)
                //    {
                //        errorList.Add(error.ErrorMessage);
                //    }
                //}

                //return BadRequest(ModelState);
            }
            return Json(person);
        }
    }
}
