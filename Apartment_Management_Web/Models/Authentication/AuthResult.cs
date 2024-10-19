namespace Apartment_Management_Web.Models.Authentication
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public UserPhong? User { get; set; }
        public string? ErrorMessage { get; set; }

    }

}
