using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Controller]
    public class BankAccountController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Json(new { message = "Welcome to the best bank" });
        }

        [Route("/account-details")]
        public IActionResult AccountDetails()
        {
            var account = new
            {
                accountNumber = 1001,
                accountHolderName = "Leandro Luz",
                currentBalance = 5000
            };
            return Json(account);
        }
        
        [Route("/account-statement")]
        [HttpGet]  
        public IActionResult AccountStatement()
        {
            byte[] statement = System.IO.File.ReadAllBytes(@"D:\Code\aspnetcore\BankApp\wwwroot\WB_instructions.pdf");
            return File(statement, "application/pdf");
        }

        [Route("/account-balance/{accountNumber:int}")]
        [HttpGet]
        public IActionResult AccountCurrentBalance()
        {
            
            var accountNumber = Request.RouteValues["accountNumber"];
            if (Convert.ToInt32(accountNumber) == 1001)
            {
                return Json(new { currenBalance = 5000 });
            }
            return NotFound(new {message = "Account Number Should be 1001"});
        }

    }
}
