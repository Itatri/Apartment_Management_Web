using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult AboutPages()
        {
            return View();
        }
    }
}
