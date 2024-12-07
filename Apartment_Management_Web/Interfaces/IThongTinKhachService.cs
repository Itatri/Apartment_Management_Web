using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Customer;

namespace Apartment_Management_Web.Interfaces
{
    public interface IThongTinKhachService
    {
        // Interface API lấy danh sách thành viên phòng 
        Task<IEnumerable<ThongTinKhach>> GetAllThongTinKhachAsync();
        // Interface API lấy thông tin khách theo CCCD và Phone 
        Task<ThongTinKhach?> GetThongTinKhachByCDDD_PhoneAsync(string cccd, string phone);
        // Interface API lấy thông tin khách theo phòng
        Task<List<ThongTinKhach?>> GetThongTinKhachByPhongAsync(string maPhong);
        // Interface API lấy thông tin thanh viên theo mã thành viên
        Task<List<ThongTinKhach?>> GetThongTinKhachByMaKhachTroAsync(string maKhachTro);
        // Interface API cập nhật thông tin thành viên
        Task<bool> UpdateThongTinKhachAsync(string maKhachTro, UpdateThongTinKhachRequest request);
        // Interface hàm Upload File chữ kí
        Task<bool> UpdateChuKyAsync(string maKhachTro, string chuKyFileName);
        // Interface hàm lấy thông tin ID khách hàng mới nhất
        Task<ThongTinKhach?> GetLastCustomerAsync();
        // Interface API kê khai thông tin thành viên phòng
        Task<ThongTinKhach> CreateCustomerAsync(ThongTinKhach customer);
        // Interface API Upload hình ảnh chữ ký thành viên phòng
        Task<string> UploadChuKyAsync(IFormFile file, string maKhachTro, string hoTen);


    }
}
