using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class BillController : Controller
    {
        public IActionResult SearchBillPage()
        {
            return View();
        }

        public IActionResult InputBill()
        {
            return View();
        }
    }
}
