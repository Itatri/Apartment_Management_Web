using Apartment_Management_Web.Models;

namespace Apartment_Management_Web.Services
{
    public interface IFeedbackService
    {
        Task<IEnumerable<FeedBack>> GetAllThongTinFeedbackAsync();
        Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong);
        Task<FeedBack?> GetLastFeedbackAsync(); // Lấy phản hồi cuối cùng
        Task<FeedBack> CreateFeedbackAsync(FeedBack feedback); // Tạo phản hồi mới
    }
}
