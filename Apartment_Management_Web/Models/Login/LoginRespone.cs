namespace Apartment_Management_Web.Models.Login
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public UserPhong User { get; set; }
    }

}
