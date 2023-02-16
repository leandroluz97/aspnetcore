﻿using Microsoft.AspNetCore.Mvc;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            //return new ViewResult() { ViewName = "abc"};
            return View();
        }
    }
}
