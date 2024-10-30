using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.User;
using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Interfaces
{
    public interface IUserPhongService
    {
        // Triển khai interface định nghĩa cho các Service CRUD, để các Services khác cũng có thể sử dụng chung các interface đã triển khai
        // ( định nghĩa ở interface IUserPhongService trước mới có thể cài đặt ở Service : UserPhongService
        Task<AuthResult> AuthenticateAsync(string id, string matKhau);
        Task<IEnumerable<UserPhong>> GetAllUserPhongsAsync();
        Task<UserPhong?> GetUserPhongByIdAsync(string id);
        Task<bool> UpdateUserPhongAsync(UserPhong userPhong);
        Task<UserRespone> DeleteUserPhongAsync(string id);
        Task<bool> CreateUserPhongAsync(UserPhong userPhong);
    }
}
