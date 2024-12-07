using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class BillController : Controller
    {
        private readonly string _apiBaseUrl;

        public BillController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public IActionResult InputBill()
        {

            ViewBag.ApiBaseUrl = _apiBaseUrl;
            return View();
        }
    }
}
