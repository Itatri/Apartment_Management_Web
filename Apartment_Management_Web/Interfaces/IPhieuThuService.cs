using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Bill;

namespace Apartment_Management_Web.Interfaces
{
    public interface IPhieuThuService
    {
        // Interface API lấy danh sách phiếu thu của phòng
        Task<IEnumerable<PhieuThu>> GetAllThongTinPhieuThuAsync();
        // Interface API lấy danh sách phiếu thu theo phòng
        Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai, int pageNumber, int pageSize);
        // Interface hàm lấy tổng phiếu thu API trả về
        Task<int> GetTotalCountAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai);
        // Interface hàm lấy thông tin phiếu thu theo mã phiếu
        Task<PhieuThu?> GetPhieuThuByMaPtAsync(string maPt);
        // Interface API cập nhật điện nước mới của phiếu thu 
        Task<bool> UpdatePhieuThuAsync(PhieuThu phieuThu);
        // Interface API xuất phiếu thu thành PDF
        Task<byte[]> ExportPhieuThuToPdfAsync(string maPt);
        // API lấy thông tin Admin của phòng
        Task<AdminInfoResponse> GetAdminInfoByMaPhongAsync(string maPhong);




    }
}
