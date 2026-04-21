using System.Diagnostics;
using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //Index View
        public IActionResult Index()
        {
            return View();
        }

        //Privacy View
        public IActionResult Privacy()
        {
            return View();
        }

        //Error View
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //ViewModel Class designed for an Error View
            var model = new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            };
            //insert Data into a View
            return View(model);
        }
    }
}
