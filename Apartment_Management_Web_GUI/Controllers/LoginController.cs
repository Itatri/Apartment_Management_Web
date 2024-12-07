using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web_GUI.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _apiBaseUrl;
        private readonly string _imageBaseUrl;

        public LoginController(IConfiguration configuration)
        {
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"];
            _imageBaseUrl = configuration["ApiSettings:ImageBaseUrl"];
        }

        public IActionResult LoginPages()
        {

            ViewBag.ApiBaseUrl = _apiBaseUrl;
            ViewBag.ImageBaseUrl = _imageBaseUrl;
            return View();
        }

        public IActionResult UserProfile()
        {


            ViewBag.ApiBaseUrl = _apiBaseUrl;
            ViewBag.ImageBaseUrl = _imageBaseUrl;
            return View();
        }



    }
}
