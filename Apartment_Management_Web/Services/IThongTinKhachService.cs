using Apartment_Management_Web.Models;
using Microsoft.Identity.Client;
using Apartment_Management_Web.Models.Authentication;
using Apartment_Management_Web.Models.User;

namespace Apartment_Management_Web.Services
{
    public interface IThongTinKhachService
    {
        Task<IEnumerable<ThongTinKhach>> GetAllThongTinKhachAsync();
        Task<ThongTinKhach?> GetThongTinKhachByCDDD_PhoneAsync(string cccd, string phone); 

    }
}
