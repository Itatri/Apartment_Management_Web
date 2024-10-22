using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Services
{
    public interface IPhieuThuService
    {
        Task<IEnumerable<PhieuThu>> GetAllThongTinPhieuThuAsync();
        Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong);
        Task<PhieuThu?> GetPhieuThuByMaPtAsync(string maPt);
        Task<bool> UpdatePhieuThuAsync(PhieuThu phieuThu);
    }
}
