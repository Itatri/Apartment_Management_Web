using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Interfaces
{
    public interface IFeedbackService
    {
        // Interface API lấy danh sách Feedback
        Task<IEnumerable<FeedBack>> GetAllThongTinFeedbackAsync();
        // Interface  API lấy danh sách Feedback theo mã phòng
        Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai, int pageNumber, int pageSize);
        // Interface hàm lấy tổng feedback API trả về của phòng
        Task<int> GetTotalFeedbackCount(string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai);
        // Interface hàm lấy tổng số feedback có trong Data
        Task<FeedBack?> GetLastFeedbackAsync();
        // Interface API gửi phản hồi 
        Task<FeedBack> CreateFeedbackAsync(FeedBack feedback);

        // Interface API lấy danh sách Feedback
        // Interface  API lấy danh sách Feedback theo mã phòng
        // Interface hàm lấy tổng feedback API trả về của phòng
        // Interface hàm lấy tổng số feedback có trong Data
        // Interface hàm lấy tổng số feedback có trong Data
        // Interface API gửi phản hồi 

    }
}
