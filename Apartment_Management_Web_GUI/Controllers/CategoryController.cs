using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult CategoryPage()
        {
            return View();
        }
    }
}
