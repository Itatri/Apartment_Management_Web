using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Services
{
    public interface IPhieuThuService
    {
        Task<IEnumerable<PhieuThu>> GetAllThongTinPhieuThuAsync();
        //Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong);
        //Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai);
        Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(
    string maPhong,
    DateOnly? startDate,
    DateOnly? endDate,
    bool? trangThai,
    int pageNumber,
    int pageSize);
        Task<int> GetTotalCountAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai);

        Task<PhieuThu?> GetPhieuThuByMaPtAsync(string maPt);
        Task<bool> UpdatePhieuThuAsync(PhieuThu phieuThu);
    }
}
