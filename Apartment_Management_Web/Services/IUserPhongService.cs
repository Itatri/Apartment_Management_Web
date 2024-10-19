using Apartment_Management_Web.Models;
using Microsoft.Identity.Client;
using Apartment_Management_Web.Models.Authentication;

namespace Apartment_Management_Web.Services
{
    public interface IUserPhongService
    {

        Task<AuthResult> AuthenticateAsync(string id, string matKhau);
        Task<IEnumerable<UserPhong>> GetAllUserPhongsAsync();
        Task<UserPhong?> GetUserPhongByIdAsync(string id);
        Task<bool> UpdateUserPhongAsync(UserPhong userPhong);
        Task<bool> DeleteUserPhongAsync(string id);
        Task<bool> CreateUserPhongAsync(UserPhong userPhong);
    }
}
