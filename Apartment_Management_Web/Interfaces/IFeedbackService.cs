using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedBack>> GetAllThongTinFeedbackAsync();
        //Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong);
        Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai);
        Task<FeedBack?> GetLastFeedbackAsync(); // Lấy phản hồi cuối cùng
        Task<FeedBack> CreateFeedbackAsync(FeedBack feedback); // Tạo phản hồi mới
    }
}
