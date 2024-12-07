using Apartment_Management_Web.Interfaces;
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

        // Hàm lấy danh sách Feedback
        public async Task<IEnumerable<FeedBack>> GetAllThongTinFeedbackAsync()
        {
            return await _context.FeedBacks.ToListAsync();
        }

        // Hàm lấy danh sách Feedback theo mã phòng

        public async Task<List<FeedBack>> GetThongTinFeedBacksBy_MaPhongAsync(
            string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai, int pageNumber, int pageSize)
        {
            var query = _context.FeedBacks.AsQueryable();
            query = query.Where(t => t.MaPhong == maPhong);

            if (startDate.HasValue)
            {
                query = query.Where(t => t.NgayGui >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.NgayGui <= endDate.Value);
            }

            if (trangThai.HasValue)
            {
                query = query.Where(t => t.TrangThai == trangThai.Value);
            }


            query = query.OrderByDescending(t => t.NgayGui);

            return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        // Hàm lấy tổng feedback API trả về của phòng
        public async Task<int> GetTotalFeedbackCount(string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai)
        {
            var query = _context.FeedBacks.AsQueryable();
            query = query.Where(t => t.MaPhong == maPhong);

            if (startDate.HasValue)
            {
                query = query.Where(t => t.NgayGui >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.NgayGui <= endDate.Value);
            }

            if (trangThai.HasValue)
            {
                query = query.Where(t => t.TrangThai == trangThai.Value);
            }

            return await query.CountAsync();
        }


        // Hàm lấy tổng số feedback có trong Data

        public async Task<FeedBack?> GetLastFeedbackAsync()
        {
            return await _context.FeedBacks
                .OrderByDescending(fb => fb.MaFb)
                .FirstOrDefaultAsync();
        }

        // Hàm gửi phản hồi 

        public async Task<FeedBack> CreateFeedbackAsync(FeedBack feedback)
        {
            _context.FeedBacks.Add(feedback);
            await _context.SaveChangesAsync();
            return feedback;
        }
    }
}
