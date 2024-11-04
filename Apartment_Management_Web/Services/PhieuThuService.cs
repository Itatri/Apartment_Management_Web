using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Microsoft.EntityFrameworkCore;
namespace Apartment_Management_Web.Services
{
    public class PhieuThuService : IPhieuThuService
    {
        private readonly QlChungCuContext _context;

        public PhieuThuService(QlChungCuContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhieuThu>> GetAllThongTinPhieuThuAsync()
        {
            return await _context.PhieuThus.ToListAsync();
        }


        //public async Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong)
        //{
        //    return await _context.PhieuThus
        //        .Where(t => t.MaPhong == maPhong)
        //        .ToListAsync();
        //}

        //public async Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai)
        //{
        //    var query = _context.PhieuThus.AsQueryable();

        //    query = query.Where(t => t.MaPhong == maPhong);

        //    // Lọc theo ngày lập
        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(t => t.NgayLap >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        query = query.Where(t => t.NgayLap <= endDate.Value);
        //    }

        //    // Lọc theo trạng thái
        //    if (trangThai.HasValue)
        //    {
        //        query = query.Where(t => t.TrangThai == trangThai.Value);
        //    }

        //    return await query.ToListAsync();
        //}


        //public async Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai, int pageNumber, int pageSize)
        //{
        //    var query = _context.PhieuThus.AsQueryable();

        //    query = query.Where(t => t.MaPhong == maPhong);

        //    // Lọc theo ngày lập
        //    if (startDate.HasValue)
        //    {
        //        query = query.Where(t => t.NgayLap >= startDate.Value);
        //    }

        //    if (endDate.HasValue)
        //    {
        //        query = query.Where(t => t.NgayLap <= endDate.Value);
        //    }

        //    // Lọc theo trạng thái
        //    if (trangThai.HasValue)
        //    {
        //        query = query.Where(t => t.TrangThai == trangThai.Value);
        //    }

        //    // Thực hiện phân trang
        //    var pagedResult = await query
        //        .Skip((pageNumber - 1) * pageSize) // Bỏ qua các mục ở trên
        //        .Take(pageSize) // Lấy số mục theo pageSize
        //        .ToListAsync();

        //    return pagedResult;
        //}

        public async Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai, int pageNumber, int pageSize)
        {
            var query = _context.PhieuThus.AsQueryable();

            query = query.Where(t => t.MaPhong == maPhong);

            // Lọc theo ngày lập
            if (startDate.HasValue)
            {
                query = query.Where(t => t.NgayLap >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.NgayLap <= endDate.Value);
            }

            // Lọc theo trạng thái
            if (trangThai.HasValue)
            {
                query = query.Where(t => t.TrangThai == trangThai.Value);
            }



            // Sắp xếp theo ngày lập giảm dần để hiển thị ngày mới nhất trước
            var pagedResult = await query
                .OrderByDescending(t => t.NgayLap) // Sắp xếp theo ngày lập
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();


            return pagedResult;
        }


        public async Task<int> GetTotalCountAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai)
        {
            var query = _context.PhieuThus.AsQueryable();

            query = query.Where(t => t.MaPhong == maPhong);

            // Lọc theo ngày lập
            if (startDate.HasValue)
            {
                query = query.Where(t => t.NgayLap >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.NgayLap <= endDate.Value);
            }

            // Lọc theo trạng thái
            if (trangThai.HasValue)
            {
                query = query.Where(t => t.TrangThai == trangThai.Value);
            }

            return await query.CountAsync();
        }





        // Lấy phiếu thu theo MaPt
        public async Task<PhieuThu?> GetPhieuThuByMaPtAsync(string maPt)
        {
            return await _context.PhieuThus
                .FirstOrDefaultAsync(pt => pt.MaPt == maPt);
        }

        // Cập nhật phiếu thu
        public async Task<bool> UpdatePhieuThuAsync(PhieuThu phieuThu)
        {
            _context.PhieuThus.Update(phieuThu);
            return await _context.SaveChangesAsync() > 0;
        }


    }
}
