using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<AboutController> _logger;
        private readonly string _apiBaseUrl; // Khai báo biến cho API Url

        public AboutController(ILogger<AboutController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"]; // Lấy URL từ cấu hình
        }

        public IActionResult AboutPages()
        {
            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }
    }
}
