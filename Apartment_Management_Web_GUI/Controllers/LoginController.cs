using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _apiBaseUrl;
        private readonly string _imageBaseUrl;
        // Khai bao APIUrl su dung chung 
        public LoginController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _imageBaseUrl = configuration["ApiSettings:ImageBaseUrl"];
        }

        public IActionResult LoginPages()
        {
            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            ViewBag.ImageBaseUrl = _imageBaseUrl;
            return View();
        }

        public IActionResult UserProfile()
        {

            // Truyền URL API vào View thông qua ViewBag
            ViewBag.ApiBaseUrl = _apiBaseUrl;
            ViewBag.ImageBaseUrl = _imageBaseUrl;
            return View();
        }



    }
}
