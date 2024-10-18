using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LoginPages()
        {
            return View();
        }

        public IActionResult RegisterPages()
        {
            return View();
        }
    }
}
