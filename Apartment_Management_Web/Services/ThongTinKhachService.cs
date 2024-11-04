using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Customer;
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

        public async Task<List<ThongTinKhach?>> GetThongTinKhachByPhongAsync(string maPhong)
        {
            return await _context.ThongTinKhaches
               .Where(t => t.MaPhong == maPhong && t.TrangThai == 1)
               .ToListAsync();
        }



        public async Task<List<ThongTinKhach?>> GetThongTinKhachByMaKhachTroAsync(string maKhachTro)
        {
            return await _context.ThongTinKhaches
               .Where(t => t.MaKhachTro == maKhachTro)
               .ToListAsync();
        }

        public async Task<bool> UpdateThongTinKhachAsync(string maKhachTro, UpdateThongTinKhachRequest request)
        {
            var thongTinKhach = await _context.ThongTinKhaches
                .FirstOrDefaultAsync(t => t.MaKhachTro == maKhachTro);

            if (thongTinKhach == null)
            {
                return false; // Không tìm thấy khách
            }

            // Cập nhật các trường thông tin từ request
            thongTinKhach.HoTen = request.HoTen ?? thongTinKhach.HoTen;
            thongTinKhach.GioiTinh = request.GioiTinh ?? thongTinKhach.GioiTinh;
            thongTinKhach.NgaySinh = request.NgaySinh ?? thongTinKhach.NgaySinh;
            thongTinKhach.Cccd = request.Cccd ?? thongTinKhach.Cccd;
            thongTinKhach.NgayCap = request.NgayCap ?? thongTinKhach.NgayCap;
            thongTinKhach.NoiCap = request.NoiCap ?? thongTinKhach.NoiCap;
            thongTinKhach.Phone = request.Phone ?? thongTinKhach.Phone;
            thongTinKhach.Email = request.Email ?? thongTinKhach.Email;
            thongTinKhach.QueQuan = request.QueQuan ?? thongTinKhach.QueQuan;
            thongTinKhach.QuanHe = request.QuanHe ?? thongTinKhach.QuanHe;
            thongTinKhach.ChuKy = request.ChuKy ?? thongTinKhach.ChuKy;
            thongTinKhach.MaPhong = request.MaPhong ?? thongTinKhach.MaPhong;
            thongTinKhach.TrangThai = request.TrangThai ?? thongTinKhach.TrangThai;

            thongTinKhach.ThuongTru = request.ThuongTru ?? thongTinKhach.ThuongTru;

            // Cập nhật vào cơ sở dữ liệu
            _context.Entry(thongTinKhach).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true; // Cập nhật thành công
        }


        public async Task<bool> UpdateChuKyAsync(string maKhachTro, string chuKyFileName)
        {
            var thongTinKhach = await _context.ThongTinKhaches
                .FirstOrDefaultAsync(t => t.MaKhachTro == maKhachTro);

            if (thongTinKhach == null)
            {
                return false; // Không tìm thấy khách
            }

            // Cập nhật tên file chữ ký
            thongTinKhach.ChuKy = chuKyFileName;

            // Cập nhật vào cơ sở dữ liệu
            _context.Entry(thongTinKhach).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true; // Cập nhật thành công
        }


        public async Task<ThongTinKhach?> GetLastCustomerAsync()
        {
            return await _context.ThongTinKhaches
                .OrderByDescending(c => c.MaKhachTro)
                .FirstOrDefaultAsync();
        }

        // Tạo khách hàng mới
        public async Task<ThongTinKhach> CreateCustomerAsync(ThongTinKhach customer)
        {
            _context.ThongTinKhaches.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

    }
}
