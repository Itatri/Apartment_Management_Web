using Apartment_Management_Web.Models;
using Microsoft.Identity.Client;
using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.User;
using Apartment_Management_Web.Models.Customer;

namespace Apartment_Management_Web.Services
{
    public interface IThongTinKhachService
    {
        Task<IEnumerable<ThongTinKhach>> GetAllThongTinKhachAsync();
        Task<ThongTinKhach?> GetThongTinKhachByCDDD_PhoneAsync(string cccd, string phone);
        Task<List<ThongTinKhach?>> GetThongTinKhachByPhongAsync(string maPhong);
        Task<List<ThongTinKhach?>> GetThongTinKhachByMaKhachTroAsync(string maKhachTro);
        Task<bool> UpdateThongTinKhachAsync(string maKhachTro, UpdateThongTinKhachRequest request);
        Task <bool> UpdateChuKyAsync(string maKhachTro, string chuKyFileName);
        Task<ThongTinKhach?> GetLastCustomerAsync();
        Task<ThongTinKhach> CreateCustomerAsync(ThongTinKhach customer);

    }
}
