using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Services
{
    public interface IUserPhongService
    {
        // Interface để định nghĩa các phương thức mà dịch vụ sẽ cung cấp:

        Task<IEnumerable<UserPhong>> GetAllUserPhongsAsync();
        Task<UserPhong?> GetUserPhongByIdAsync(string id);
        Task<UserPhong?> AuthenticateAsync(string id, string matKhau);
        Task<bool> UpdateUserPhongAsync(UserPhong userPhong);
        Task<bool> DeleteUserPhongAsync(string id);
        Task<bool> CreateUserPhongAsync(UserPhong userPhong);

    }
}
