using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;
using Autofac;

namespace DependencyEjectionExample.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICitiesService _citiesService;
        private readonly ICitiesService _citiesService2;
        private readonly ICitiesService _citiesService3;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILifetimeScope _lifetimeScope; //AutoFac
        public HomeController(
            ICitiesService citiesService, 
            ICitiesService citiesService2, 
            ICitiesService citiesService3,
            IServiceScopeFactory serviceScopeFactory,
            ILifetimeScope lifetimeScope
            )
        {
            _citiesService = citiesService;
            _citiesService2 = citiesService2;
            _citiesService3 = citiesService3;
            _serviceScopeFactory = serviceScopeFactory;
            _lifetimeScope = lifetimeScope;
        }
        [Route("/")]
        //public IActionResult Index([FromServices] ICitiesService _citiesService)
        public IActionResult Index()
        {
            List<string> cities =  _citiesService.GetCities();
            ViewBag.Instanced_Id_CitiesService = _citiesService.ServiceInstanceId;
            ViewBag.Instanced_Id_CitiesService2 = _citiesService2.ServiceInstanceId;
            ViewBag.Instanced_Id_CitiesService3 = _citiesService3.ServiceInstanceId;

            using (ILifetimeScope scope = _lifetimeScope.BeginLifetimeScope())
            {
                // Inject CitiesService
                ICitiesService citiesService = scope.Resolve<ICitiesService>();
                //DB work

                ViewBag.Instanced_Id_CitiesService_InScope = citiesService.ServiceInstanceId;
            }   //e

            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                // Inject CitiesService
                ICitiesService citiesService  =  scope.ServiceProvider.GetService<ICitiesService>();
                //DB work

                ViewBag.Instanced_Id_CitiesService_InScope = citiesService.ServiceInstanceId;
            }   //end of scope it calls CitiesService.Dispose()
            return View(cities);
        }
    }
}
