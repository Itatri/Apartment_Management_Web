using Apartment_Management_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartment_Management_Web.Services
{
    public class ThongTinKhachService : IThongTinKhachService
    {
        // Sửa lại DBContext nếu có thay đổi DB
        private readonly QlChungCuContext _context;

        public ThongTinKhachService(QlChungCuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ThongTinKhach>> GetAllThongTinKhachAsync()
        {
            return await _context.ThongTinKhaches.ToListAsync();
        }

        public async Task<ThongTinKhach?> GetThongTinKhachByCDDD_PhoneAsync(string cccd, string phone)
        {
            return await _context.ThongTinKhaches
                .FirstOrDefaultAsync(t => t.Cccd == cccd && t.Phone == phone);
        }

    }
}
