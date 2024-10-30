using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly string _apiBaseUrl;
        // Khai bao APIUrl su dung chung 
        public FeedbackController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public IActionResult FeedbackPage()
        {
            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }
    }
}
