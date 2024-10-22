namespace Apartment_Management_Web.Models.User
{
    public class UserRespone
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserPhong? User { get; set; } // Nếu cần thông tin người dùng đã xóa

    }

    public class UpdateUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserPhong UpdatedUser { get; set; } // Bạn có thể trả về thông tin người dùng đã cập nhật, nếu cần
    }

}
