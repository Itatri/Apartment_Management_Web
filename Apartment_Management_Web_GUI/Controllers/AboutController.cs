using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly ILogger<AboutController> _logger;
        private readonly string _apiBaseUrl;

        public AboutController(ILogger<AboutController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public IActionResult AboutPages()
        {

            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }
    }
}
