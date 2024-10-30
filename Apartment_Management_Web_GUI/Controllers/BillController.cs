using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class BillController : Controller
    {
        private readonly string _apiBaseUrl;
        // Khai bao APIUrl su dung chung 
        public BillController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public IActionResult SearchBillPage()
        {
            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }

        public IActionResult InputBill()
        {
            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }
    }
}
