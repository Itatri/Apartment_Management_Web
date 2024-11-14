using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.User;
using Microsoft.EntityFrameworkCore;


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

        public async Task<Phong?> GetPhongByMaPhongAsync(string maPhong)
        {
            return await _context.Phongs.FindAsync(maPhong);
        }



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



        public async Task<UserRespone> DeleteUserPhongAsync(string id)
        {
            var userPhong = await _context.UserPhongs.FindAsync(id);
            if (userPhong == null)
            {
                return new UserRespone
                {
                    IsSuccess = false,
                    Message = "Người dùng không tồn tại.",
                    User = null
                };
            }

            _context.UserPhongs.Remove(userPhong);
            await _context.SaveChangesAsync();

            return new UserRespone
            {
                IsSuccess = true,
                Message = "Người dùng đã được xóa thành công.",
                User = userPhong // Có thể trả về thông tin người dùng đã xóa nếu cần
            };
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
