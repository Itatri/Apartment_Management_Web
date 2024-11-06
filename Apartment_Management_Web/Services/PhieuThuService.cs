using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;  // Thêm dòng này
using iText.Layout.Element;
using iText.Layout.Properties;
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

        // Phương thức xuất phiếu thu thành PDF
        public async Task<byte[]> ExportPhieuThuToPdfAsync(string maPt)
        {
            // Lấy thông tin phiếu thu từ database
            var phieuThu = await _context.PhieuThus.FirstOrDefaultAsync(pt => pt.MaPt == maPt);

            if (phieuThu == null)
            {
                throw new Exception("Không tìm thấy phiếu thu.");
            }

            string fontPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "ARIAL.TTF");


            using (var ms = new MemoryStream())
            {
                using (var writer = new PdfWriter(ms))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf, PageSize.A4);

                        // Áp dụng font hỗ trợ tiếng Việt
                        var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                        // Tiêu đề Hóa đơn
                        document.Add(new Paragraph("HÓA ĐƠN PHIẾU THU")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBold()
                            .SetFont(font)
                            .SetFontSize(20));

                        document.Add(new Paragraph($"Mã Phiếu Thu: {phieuThu.MaPt}")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Mã Phòng: {phieuThu.MaPhong}")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Ngày Lập: {phieuThu.NgayLap?.ToString("dd/MM/yyyy")}")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Ngày Thu: {phieuThu.NgayThu?.ToString("dd/MM/yyyy")}")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));

                        // Thêm thông tin các khoản phí
                        document.Add(new Paragraph($"Tiền Nhà: {phieuThu.TienNha?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Tiền Điện: {phieuThu.TienDien?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Tiền Nước: {phieuThu.TienNuoc?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Tiền Dịch Vụ: {phieuThu.TienDv?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));

                        // Tính tổng tiền
                        document.Add(new Paragraph($"Tổng Tiền: {phieuThu.TongTien?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));

                        // Thêm thông tin thanh toán
                        document.Add(new Paragraph($"Số Tiền Đã Thanh Toán: {phieuThu.ThanhToan?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetFont(font)
                            .SetFontSize(12));

                        // Kết thúc document
                        document.Close();
                    }
                }

                return ms.ToArray(); // Trả về file PDF dưới dạng byte array
            }
        }


    }
}
