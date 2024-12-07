using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly string _apiBaseUrl;

        public FeedbackController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public IActionResult FeedbackPage()
        {

            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }
    }
}
