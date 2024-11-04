using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Customer;

namespace Apartment_Management_Web.Interfaces
{
    public interface IThongTinKhachService
    {
        Task<IEnumerable<ThongTinKhach>> GetAllThongTinKhachAsync();
        Task<ThongTinKhach?> GetThongTinKhachByCDDD_PhoneAsync(string cccd, string phone);
        Task<List<ThongTinKhach?>> GetThongTinKhachByPhongAsync(string maPhong);
        Task<List<ThongTinKhach?>> GetThongTinKhachByMaKhachTroAsync(string maKhachTro);
        Task<bool> UpdateThongTinKhachAsync(string maKhachTro, UpdateThongTinKhachRequest request);
        Task<bool> UpdateChuKyAsync(string maKhachTro, string chuKyFileName);
        Task<ThongTinKhach?> GetLastCustomerAsync();
        Task<ThongTinKhach> CreateCustomerAsync(ThongTinKhach customer);

        Task<string> UploadChuKyAsync(IFormFile file, string maKhachTro, string hoTen);
    }
}
