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

        //public async Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong)
        //{
        //    return await _context.FeedBacks
        //        .Where(t => t.MaPhong == maPhong)
        //        .ToListAsync();
        //}

        public async Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai)
        {
            var query = _context.FeedBacks.AsQueryable();

            query = query.Where(t => t.MaPhong == maPhong);

            // Lọc theo ngày gửi
            if (startDate.HasValue)
            {
                query = query.Where(t => t.NgayGui >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.NgayGui <= endDate.Value);
            }

            // Lọc theo trạng thái
            if (trangThai.HasValue)
            {
                query = query.Where(t => t.TrangThai == trangThai.Value);
            }

            return await query.ToListAsync();
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
