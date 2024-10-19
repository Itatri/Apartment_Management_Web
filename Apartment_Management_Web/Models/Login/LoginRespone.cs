namespace Apartment_Management_Web.Models.Login
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; } // Trạng thái đăng nhập thành công hay không
        public string Token { get; set; }    // Token nếu đăng nhập thành công
        public string Message { get; set; }   // Thông điệp về tình trạng
        public UserPhong User { get; set; }   // Thông tin người dùng (nếu cần)
    }

}
