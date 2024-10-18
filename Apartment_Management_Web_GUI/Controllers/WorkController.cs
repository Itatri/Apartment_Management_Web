using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class WorkController : Controller
    {
        public IActionResult WorkPages()
        {
            return View();
        }
    }
}
