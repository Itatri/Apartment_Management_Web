using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Bill;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
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

        public async Task<List<DichVuPhieuThu>> GetDichVuByMaPtAsync(string maPt)
        {
            return await _context.DichVuPhieuThus
                .Where(dv => dv.MaPt == maPt)
                .ToListAsync();
        }

        public async Task<byte[]> ExportPhieuThuToPdfAsync(string maPt)
        {


            var phieuThu = await _context.PhieuThus.FirstOrDefaultAsync(pt => pt.MaPt == maPt);
            if (phieuThu == null)
            {
                throw new Exception("Không tìm thấy phiếu thu.");
            }

            var phong = await _context.Phongs.FirstOrDefaultAsync(p => p.MaPhong == phieuThu.MaPhong);
            string tenPhong = phong?.TenPhong ?? "Không xác định";
            double? congNo = phong?.CongNo; // Lấy giá trị Công Nợ từ bảng Phong
            double? congNoHienThi = congNo.HasValue ? -congNo : 0;

            var dichVuList = await _context.DichVuPhieuThus
                .Where(dv => dv.MaPt == maPt)
                .ToListAsync();

            string fontPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fonts", "ARIAL.TTF");
            string logoPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "apartment.png");

            using (var ms = new MemoryStream())
            {
                using (var writer = new PdfWriter(ms))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var document = new Document(pdf, PageSize.A4);
                        var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);





                        // Thông tin cơ bản
                        document.Add(new Paragraph("HÓA ĐƠN TIỀN PHÒNG")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetBold()
                            .SetFont(font)
                            .SetFontSize(20));

                        document.Add(new Paragraph($"Mã Phiếu Thu : {phieuThu.MaPt}")
                            .SetFont(font).SetFontSize(12));


                        // Tạo một Paragraph cho phòng và trạng thái thanh toán cùng một dòng
                        var roomStatusLine = new Paragraph()

                            .Add(new Text($"Phòng : {tenPhong}")  // Hiển thị tên phòng
                            .SetFont(font)
                            .SetFontSize(12))
                            .Add(new Text("     ") // Khoảng cách giữa Phòng và Trạng Thái
                                .SetFont(font)
                                .SetFontSize(12))
                            .Add(new Text($"Trạng Thái : ")
                                .SetFont(font)
                                .SetFontSize(12))
                            .Add(new Text(phieuThu.TrangThai == true ? "Đã thanh toán" : "Chưa thanh toán")
                                .SetFont(font)
                                .SetFontSize(12)
                                .SetFontColor(phieuThu.TrangThai == true ? ColorConstants.GREEN : ColorConstants.RED));

                        // Thêm đoạn văn vào tài liệu
                        document.Add(roomStatusLine);


                        // Thêm ngày lập và ngày thu vào cùng một dòng
                        var dateLine = new Paragraph()
                            .Add(new Text($"Ngày Lập : {phieuThu.NgayLap?.ToString("dd/MM/yyyy")}").SetFont(font).SetFontSize(12))
                            .Add(new Text($"     Ngày Thu : {phieuThu.NgayThu?.ToString("dd/MM/yyyy")}").SetFont(font).SetFontSize(12));

                        document.Add(dateLine);

                        // Kiểm tra nếu congNo có giá trị, nếu có thì gọi ToString, nếu không thì sử dụng chuỗi trống hoặc giá trị mặc định
                        document.Add(new Paragraph($"Số tiền còn nợ : {congNoHienThi?.ToString("#,0") ?? "0"} VND")
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFont(font)
                        .SetFontSize(12));

                        int stt = 1;




                        // Thêm khoảng cách trước bảng Phiếu Thu
                        document.Add(new Paragraph("\n"));

                        // Thêm bảng Phiếu Thu tiền phòng
                        var phieuThuTable = new Table(7); // Bảng có 7 cột
                        phieuThuTable.SetWidth(UnitValue.CreatePercentValue(100)); // Đặt độ rộng bảng là 100% chiều rộng trang

                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("STT").SetFont(font).SetBold()));
                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("Khoản").SetFont(font).SetBold()));
                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("Chỉ số đầu").SetFont(font).SetBold()));
                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("Chỉ số cuối").SetFont(font).SetBold()));
                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("Số lượng").SetFont(font).SetBold()));
                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá").SetFont(font).SetBold()));
                        phieuThuTable.AddHeaderCell(new Cell().Add(new Paragraph("Thành Tiền").SetFont(font).SetBold()));

                        stt = 1;

                        // Thêm các dịch vụ cố định như Tiền Phòng
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph(stt.ToString()).SetFont(font)));
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph("Tiền Phòng").SetFont(font)));
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph("-")).SetFont(font));
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph("-")).SetFont(font));
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph("1").SetFont(font)));
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.TienNha?.ToString("#,0")} ").SetFont(font)));
                        phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.TienNha?.ToString("#,0")} ").SetFont(font)));
                        stt++;



                        // Thêm các dịch vụ vào bảng Phiếu Thu
                        foreach (var dichVu in dichVuList)
                        {
                            // Kiểm tra nếu là dịch vụ điện hoặc nước
                            if (dichVu.TenDichVu == "Dịch vụ điện")
                            {
                                // Cập nhật dòng Tiền Điện
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph(stt.ToString()).SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph("Tiền Điện").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.DienCu?.ToString()}").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.DienMoi?.ToString()}").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{(phieuThu.DienMoi - phieuThu.DienCu)?.ToString()}").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{dichVu.DonGia?.ToString("#,0") ?? "-"}").SetFont(font))); // Đơn giá Tiền Điện
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.TienDien?.ToString("#,0")} ").SetFont(font))); // Thành tiền
                                stt++;
                            }
                            else if (dichVu.TenDichVu == "Dịch vụ nước")
                            {
                                // Cập nhật dòng Tiền Nước
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph(stt.ToString()).SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph("Tiền Nước").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.NuocCu?.ToString()}").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.NuocMoi?.ToString()}").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{(phieuThu.NuocMoi - phieuThu.NuocCu)?.ToString()}").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{dichVu.DonGia?.ToString("#,0") ?? "-"}").SetFont(font))); // Đơn giá Tiền Nước
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{phieuThu.TienNuoc?.ToString("#,0")} ").SetFont(font))); // Thành tiền
                                stt++;
                            }
                            else
                            {
                                // Các dịch vụ còn lại hiển thị bình thường
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph(stt.ToString()).SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph(dichVu.TenDichVu ?? "-").SetFont(font)));
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph("-").SetFont(font))); // Chỉ số đầu
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph("-").SetFont(font))); // Chỉ số cuối
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph(dichVu.SoLuong?.ToString() ?? "-").SetFont(font))); // Số lượng
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph(dichVu.DonGia?.ToString("#,0") ?? "-").SetFont(font))); // Đơn giá
                                phieuThuTable.AddCell(new Cell().Add(new Paragraph($"{((dichVu.DonGia ?? 0) * 1).ToString("#,0")}"))); // Thành tiền có dấu phân cách
                                stt++;
                            }
                        }



                        document.Add(new Paragraph("PHIẾU THU TIỀN PHÒNG")
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetBold()
                            .SetFont(font)
                            .SetFontSize(12));

                        document.Add(phieuThuTable);

                        document.Add(new Paragraph("\n"));






                        // Tổng tiền và kết thúc
                        document.Add(new Paragraph($"Tổng Tiền: {phieuThu.TongTien?.ToString("#,0")} VND")
                            .SetTextAlignment(TextAlignment.RIGHT).SetFont(font).SetBold().SetFontSize(12));





                        document.Add(new Paragraph("\n"));

                        // Thêm dòng yêu cầu và thông tin liên hệ, căn giữa
                        document.Add(new Paragraph("* Yêu cầu các phòng thanh toán trước thời gian quy định")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(font)
                            .SetFontColor(ColorConstants.RED)
                            .SetFontSize(12));

                        document.Add(new Paragraph("Mọi thông tin liên hệ quản lý theo SĐT: 0328319660")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(font)
                            .SetFontSize(12));

                        document.Add(new Paragraph("Chân thành cảm ơn !")
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(font)
                            .SetFontSize(12));

                        // Thêm khoảng cách giữa logo và tiêu đề
                        document.Add(new Paragraph("\n"));
                        // Thêm logo vào đầu trang, căn giữa
                        var logoImage = new Image(ImageDataFactory.Create(logoPath));
                        logoImage.SetHorizontalAlignment(HorizontalAlignment.CENTER);
                        logoImage.ScaleToFit(30, 30); // Điều chỉnh kích thước logo 
                        document.Add(logoImage);

                        document.Close();
                    }
                }

                return ms.ToArray();
            }
        }

        public async Task<AdminInfoResponse> GetAdminInfoByMaPhongAsync(string maPhong)
        {
            var phong = await _context.Phongs.FirstOrDefaultAsync(p => p.MaPhong == maPhong);
            if (phong == null)
            {
                return new AdminInfoResponse { IsSuccess = false, Message = "Không tìm thấy phòng" };
            }

            var dangNhap = await _context.DangNhaps.FirstOrDefaultAsync(d => d.MaKhuVuc == phong.MaKhuVuc);
            if (dangNhap == null)
            {
                return new AdminInfoResponse { IsSuccess = false, Message = "Không tìm thấy tài khoản đăng nhập cho khu vực" };
            }

            var admin = await _context.ThongTinAdmins.FirstOrDefaultAsync(a => a.IdUser == dangNhap.Id);
            if (admin == null)
            {
                return new AdminInfoResponse { IsSuccess = false, Message = "Không tìm thấy thông tin Admin" };
            }

            // Lấy thông tin cần thiết
            var adminDto = new AdminInfoDto
            {
                MaAdmin = admin.MaAdmin,
                HoTen = admin.HoTen,
                GioiTinh = admin.GioiTinh,
                NgaySinh = admin.NgaySinh.HasValue ? admin.NgaySinh.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                CCCD = admin.Cccd,
                DiaChi = admin.DiaChi,
                Phone = admin.Phone,
                ChuKy = admin.ChuKy,
                NganHang = admin.NganHang,
                TaiKhoan = admin.TaiKhoan,
                IdUser = admin.IdUser
            };

            return new AdminInfoResponse
            {
                IsSuccess = true,
                Message = "Lấy thông tin Admin thành công",
                AdminInfo = adminDto
            };
        }







    }
}
