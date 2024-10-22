using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult FeedbackPage()
        {
            return View();
        }
    }
}
