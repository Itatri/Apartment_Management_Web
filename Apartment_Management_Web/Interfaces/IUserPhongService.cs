using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.User;

namespace Apartment_Management_Web.Interfaces
{
    public interface IUserPhongService
    {
        // Interface API kiểm tra đăng nhập vào trang web
        Task<AuthResult> AuthenticateAsync(string id, string matKhau);
        // Interface API lấy danh sách tài khoản phòng 
        Task<IEnumerable<UserPhong>> GetAllUserPhongsAsync();
        // Interface API lấy  tài khoản phòng theo Id
        Task<UserPhong?> GetUserPhongByIdAsync(string id);
        // Interface API cập nhật tài khoản phòng
        Task<bool> UpdateUserPhongAsync(UserPhong userPhong);
        // Interface  API xóa thông tin tài khoản phòng  
        Task<UserRespone> DeleteUserPhongAsync(string id);
        // Interface API tạo thông tin tài khoản phòng mới 
        Task<bool> CreateUserPhongAsync(UserPhong userPhong);
        // Interface API lấy thông tin phòng theo mã phòng
        Task<Phong?> GetPhongByMaPhongAsync(string maPhong);


    }
}
