using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Bill;
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

      
        public async Task<List<PhieuThu>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong)
        {
            return await _context.PhieuThus
                .Where(t => t.MaPhong == maPhong)
                .ToListAsync();
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
