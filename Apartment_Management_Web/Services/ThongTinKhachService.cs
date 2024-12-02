using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Customer;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;


namespace Apartment_Management_Web.Services
{
    public class ThongTinKhachService : IThongTinKhachService
    {
        private readonly IConfiguration _configuration;

        private readonly QlChungCuContext _context;

        public ThongTinKhachService(QlChungCuContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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




        public async Task<string> UploadChuKyAsync(IFormFile file, string maKhachTro, string hoTen)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Không có tệp nào được tải lên.");
            }

            // Đọc đường dẫn từ appsettings.json
            var folderPath = _configuration["ImageSettings:UploadFolderPath"];

            // Đảm bảo thư mục images tồn tại
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Kiểm tra và tạo thư mục ChuKy trong thư mục images
            var chuKyFolderPath = Path.Combine(folderPath, "ChuKy");
            if (!Directory.Exists(chuKyFolderPath))
            {
                Directory.CreateDirectory(chuKyFolderPath); // Tạo thư mục ChuKy nếu chưa có
            }

            // Xử lý họ tên để loại bỏ dấu và khoảng trắng
            var tenKhongDau = RemoveDiacritics(hoTen).Replace(" ", "");

            // Đặt tên file theo định dạng CK_MaKhachTro_HoTen.jpg
            var fileName = $"CK_{maKhachTro}_{tenKhongDau}.jpg";
            var filePath = Path.Combine(chuKyFolderPath, fileName);  // Lưu tệp trong thư mục ChuKy

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream); // Lưu tệp vào thư mục
            }

            // Ghi tên file vào database
            await UpdateChuKyAsync(maKhachTro, fileName);

            return fileName; // Trả về tên file đã upload
        }


        // Phương thức để loại bỏ dấu trong chuỗi
        public string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
