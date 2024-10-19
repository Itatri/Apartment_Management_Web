using Apartment_Management_Web.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apartment_Management_Web.Models.Authentication;


namespace Apartment_Management_Web.Services
{
    // Tạo lớp UserPhongService để triển khai IUserPhongService
    public class UserPhongService : IUserPhongService
    {
        // Sửa lại DBContext nếu có thay đổi DB
        private readonly QlChungCuContext _context;

        public UserPhongService(QlChungCuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserPhong>> GetAllUserPhongsAsync()
        {
            return await _context.UserPhongs.ToListAsync();
        }

        public async Task<UserPhong?> GetUserPhongByIdAsync(string id)
        {
            return await _context.UserPhongs.FindAsync(id);
        }

        //public async Task<UserPhong?> AuthenticateAsync(string id, string matKhau)
        //{
        //    return await _context.UserPhongs
        //        .SingleOrDefaultAsync(u => u.Id == id && u.MatKhau == matKhau);
        //}

        public async Task<AuthResult> AuthenticateAsync(string id, string matKhau)
        {
            var user = await _context.UserPhongs.SingleOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Tài khoản không tồn tại.",
                    User = null // Không có người dùng
                };
            }

            if (user.MatKhau != matKhau)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Mật khẩu không đúng.",
                    User = null // Không có người dùng
                };
            }

            return new AuthResult
            {
                IsSuccess = true,
                User = user // Trả về người dùng nếu xác thực thành công
            };
        }




        public async Task<bool> UpdateUserPhongAsync(UserPhong userPhong)
        {
            _context.Entry(userPhong).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserPhongAsync(string id)
        {
            var userPhong = await _context.UserPhongs.FindAsync(id);
            if (userPhong == null) return false;

            _context.UserPhongs.Remove(userPhong);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateUserPhongAsync(UserPhong userPhong)
        {
            _context.UserPhongs.Add(userPhong);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

    }
}
