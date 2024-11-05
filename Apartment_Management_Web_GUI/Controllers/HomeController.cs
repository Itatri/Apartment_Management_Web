using Apartment_Management_Web_GUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _apiBaseUrl; // Khai báo biến cho API Url


        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]; // Lấy URL từ cấu hình
        }
        // Run Home Page
        public IActionResult HomePage()
        {
            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }

        public IActionResult HomePageLogin()
        {
            // Truyền URL API vào View thông qua ViewBag
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
