using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MunicipalServicesMVP.Models;

namespace MunicipalServicesMVP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Main menu page with three municipal service options
        /// </summary>
        public IActionResult Index()
        {
            // Set up the three main menu options as per requirements
            ViewBag.ReportIssuesEnabled = true;        // Active functionality
            ViewBag.LocalEventsEnabled = false;        // To be implemented later
            ViewBag.ServiceStatusEnabled = false;      // To be implemented later
            
            ViewBag.Title = "Municipal Services Portal";
            ViewBag.WelcomeMessage = "Welcome to the Municipal Services Portal. Please select a service below.";
            
            return View();
        }

        /// <summary>
        /// About page with information about the municipal services
        /// </summary>
        public IActionResult About()
        {
            ViewBag.Title = "About Municipal Services";
            return View();
        }

        /// <summary>
        /// Contact information page
        /// </summary>
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
