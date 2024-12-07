using Apartment_Management_Web_GUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _apiBaseUrl;


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public IActionResult HomePage()
        {

            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }

        public IActionResult HomePageLogin()
        {

            ViewBag.ApiBaseUrl = _apiBaseUrl;
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
