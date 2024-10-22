using Apartment_Management_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Apartment_Management_Web.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly QlChungCuContext _context;

        public FeedbackService(QlChungCuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FeedBack>> GetAllThongTinFeedbackAsync()
        {
            return await _context.FeedBacks.ToListAsync();
        }

        public async Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong)
        {
            return await _context.FeedBacks
                .Where(t => t.MaPhong == maPhong)
                .ToListAsync();
        }

        // Lấy phản hồi cuối cùng để tính số thứ tự
        public async Task<FeedBack?> GetLastFeedbackAsync()
        {
            return await _context.FeedBacks
                .OrderByDescending(fb => fb.MaFb)
                .FirstOrDefaultAsync();
        }

        // Tạo phản hồi mới
        public async Task<FeedBack> CreateFeedbackAsync(FeedBack feedback)
        {
            _context.FeedBacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }
    }
}
