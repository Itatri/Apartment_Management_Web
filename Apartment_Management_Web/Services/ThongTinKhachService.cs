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
        // Hàm lấy danh sách thành viên phòng 

        public async Task<IEnumerable<ThongTinKhach>> GetAllThongTinKhachAsync()
        {
            return await _context.ThongTinKhaches.ToListAsync();
        }
        // Hàm lấy thông tin khách theo CCCD và Phone 

        public async Task<ThongTinKhach?> GetThongTinKhachByCDDD_PhoneAsync(string cccd, string phone)
        {
            return await _context.ThongTinKhaches
                .FirstOrDefaultAsync(t => t.Cccd == cccd && t.Phone == phone);
        }
        // Hàm lấy thông tin khách theo phòng

        public async Task<List<ThongTinKhach?>> GetThongTinKhachByPhongAsync(string maPhong)
        {
            return await _context.ThongTinKhaches
               .Where(t => t.MaPhong == maPhong && t.TrangThai == 1)
               .ToListAsync();
        }

        // Hàm lấy thông tin thanh viên theo mã thành viên


        public async Task<List<ThongTinKhach?>> GetThongTinKhachByMaKhachTroAsync(string maKhachTro)
        {
            return await _context.ThongTinKhaches
               .Where(t => t.MaKhachTro == maKhachTro)
               .ToListAsync();
        }
        // Hàm cập nhật thông tin thành viên

        public async Task<bool> UpdateThongTinKhachAsync(string maKhachTro, UpdateThongTinKhachRequest request)
        {
            var thongTinKhach = await _context.ThongTinKhaches
                .FirstOrDefaultAsync(t => t.MaKhachTro == maKhachTro);

            if (thongTinKhach == null)
            {
                return false;
            }


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


            _context.Entry(thongTinKhach).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        // Hàm  Upload File chữ kí

        public async Task<bool> UpdateChuKyAsync(string maKhachTro, string chuKyFileName)
        {
            var thongTinKhach = await _context.ThongTinKhaches
                .FirstOrDefaultAsync(t => t.MaKhachTro == maKhachTro);

            if (thongTinKhach == null)
            {
                return false;
            }


            thongTinKhach.ChuKy = chuKyFileName;


            _context.Entry(thongTinKhach).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        // Hàm  lấy thông tin ID khách hàng mới nhất

        public async Task<ThongTinKhach?> GetLastCustomerAsync()
        {
            return await _context.ThongTinKhaches
                .OrderByDescending(c => c.MaKhachTro)
                .FirstOrDefaultAsync();
        }

        // Hàm API kê khai thông tin thành viên phòng

        public async Task<ThongTinKhach> CreateCustomerAsync(ThongTinKhach customer)
        {
            _context.ThongTinKhaches.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }



        // Hàm API Upload hình ảnh chữ ký thành viên phòng
        public async Task<string> UploadChuKyAsync(IFormFile file, string maKhachTro, string hoTen)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Không có tệp nào được tải lên.");
            }


            var folderPath = _configuration["ImageSettings:UploadFolderPath"];


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var chuKyFolderPath = Path.Combine(folderPath, "ChuKy");
            if (!Directory.Exists(chuKyFolderPath))
            {
                Directory.CreateDirectory(chuKyFolderPath);
            }


            var tenKhongDau = RemoveDiacritics(hoTen).Replace(" ", "");


            var fileName = $"CK_{maKhachTro}_{tenKhongDau}.jpg";
            var filePath = Path.Combine(chuKyFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            await UpdateChuKyAsync(maKhachTro, fileName);

            return fileName;
        }


        // Hàm loại bỏ dấu và khoảng trống chuỗi
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
