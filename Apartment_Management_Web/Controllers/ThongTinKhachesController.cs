using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Services;
using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models.Customer;
using System.Text.RegularExpressions; // Thêm namespace này nếu chưa có
using System.Text; // Để sử dụng StringBuilder
using System.Globalization; // Để sử dụng CharUnicodeInfo và UnicodeCategory
using Microsoft.AspNetCore.Authorization;



namespace Apartment_Management_Web.Controllers
{
    [Route("api/ThongTinKhaches")]
    [ApiController]
    public class ThongTinKhachesController : ControllerBase
    {
        private readonly IThongTinKhachService _ThongTinKhachService;

        public ThongTinKhachesController(IThongTinKhachService ThongTinKhachService)
        {
            _ThongTinKhachService = ThongTinKhachService;
        }

        // API lấy danh sách User phòng 
        [Authorize]
        [HttpGet("GetAllThongTinKhach")]
        public async Task<ActionResult<IEnumerable<ThongTinKhach>>> GetThongTinKhachs()
        {
            var thongtinKhach = await _ThongTinKhachService.GetAllThongTinKhachAsync();
            return Ok(thongtinKhach);
        }



        // API lấy thông tin khách theo CCCD và Phone
        [Authorize]
        [HttpGet("GetThongTinKhachByCDDD_Phone")]
        public async Task<ActionResult<APICustomerRespone>> GetThongTinKhachByCDDD_Phone(string cccd, string phone)
        {
            var thongTinKhach = await _ThongTinKhachService.GetThongTinKhachByCDDD_PhoneAsync(cccd, phone);

            var response = new APICustomerRespone();

            if (thongTinKhach == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thông tin khách.";
                response.Khach = null;
                return NotFound(response); // Trả về trạng thái 404 với response
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin khách thành công.";
            response.Khach = thongTinKhach; // Trả về thông tin khách tìm thấy

            return Ok(response); // Trả về trạng thái 200 với response
        }

        // API lấy thông tin khách theo CCCD và Phone
        [Authorize]
        [HttpGet("GetThongTinKhachByPhong")]
        public async Task<ActionResult<APICustomerRespone>> GetThongTinKhachByPhong(string maPhong)
        {
            var thongTinKhach = await _ThongTinKhachService.GetThongTinKhachByPhongAsync(maPhong);

            var response = new APICustomerRespone();

            if (thongTinKhach == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thông tin khách.";
                response.Khachs = null;
                return NotFound(response); // Trả về trạng thái 404 với response
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin khách thành công.";
            response.Khachs = thongTinKhach; // Trả về thông tin khách tìm thấy

            return Ok(response); // Trả về trạng thái 200 với response
        }



        // API lấy thông tin khách theo CCCD và Phone
        [Authorize]
        [HttpGet("GetThongTinKhachByMaKhachTro")]
        public async Task<ActionResult<APICustomerRespone>> GetThongTinKhachByMaKhachTro(string maKhachTro)
        {
            var thongTinKhach = await _ThongTinKhachService.GetThongTinKhachByMaKhachTroAsync(maKhachTro);

            var response = new APICustomerRespone();

            if (thongTinKhach == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thông tin khách.";
                response.Khachs = null;
                return NotFound(response); // Trả về trạng thái 404 với response
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin khách thành công.";
            response.Khachs = thongTinKhach; // Trả về thông tin khách tìm thấy

            return Ok(response); // Trả về trạng thái 200 với response
        }



        [HttpPut("UpdateThongTinKhach")]
        [Authorize]
        public async Task<ActionResult<APICustomerRespone>> UpdateThongTinKhach(string maKhachTro, [FromBody] UpdateThongTinKhachRequest request)
        {
            var isUpdated = await _ThongTinKhachService.UpdateThongTinKhachAsync(maKhachTro, request);

            if (!isUpdated)
            {
                return NotFound(new APICustomerRespone
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy thông tin khách.",
                    Khachs = null
                });
            }

            return Ok(new APICustomerRespone
            {
                IsSuccess = true,
                Message = "Cập nhật thông tin khách thành công.",
                Khachs = null // Bạn có thể trả về thông tin khách đã cập nhật nếu cần
            });
        }

        [HttpPost("CreateThongTinKhach")]
        [Authorize]
        public async Task<ActionResult<APICustomerRespone>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var response = new APICustomerRespone();

            try
            {
                // Tự động tạo MaKhachTro với tiền tố KT + số thứ tự
                var lastCustomer = await _ThongTinKhachService.GetLastCustomerAsync();
                int nextNumber = 1;

                if (lastCustomer != null)
                {
                    // Lấy số thứ tự từ MaKhachTro cuối cùng
                    var lastNumber = int.Parse(lastCustomer.MaKhachTro.Substring(2));
                    nextNumber = lastNumber + 1;
                }

                // Tạo mã khách mới
                string maKhachTro = "KH" + nextNumber.ToString("D3");

                // Tạo đối tượng ThongTinKhach mới
                var newCustomer = new ThongTinKhach
                {
                    MaKhachTro = maKhachTro,
                    HoTen = request.HoTen,
                    GioiTinh = request.GioiTinh,
                    NgaySinh = request.NgaySinh,
                    Cccd = request.Cccd,
                    NgayCap = request.NgayCap,
                    NoiCap = request.NoiCap,
                    Phone = request.Phone,
                    Email = request.Email,
                    QueQuan = request.QueQuan,
                    QuanHe = request.QuanHe,
                    MaPhong = request.MaPhong,
                    ChuKy = request.ChuKy, // Gán tên tệp chữ ký vào đối tượng
                    ThuongTru = request.ThuongTru,
                    TrangThai = 1 // Set trạng thái mặc định là 1 (đang hoạt động)
                };

                // Gọi service để thêm khách hàng mới vào database
                var createdCustomer = await _ThongTinKhachService.CreateCustomerAsync(newCustomer);

                response.IsSuccess = true;
                response.Message = "Tạo khách hàng mới thành công.";
                response.Khachs = new List<ThongTinKhach> { createdCustomer };

                return Ok(response); // Trả về 200 với response
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Đã xảy ra lỗi khi tạo khách hàng mới: " + ex.Message;
                return BadRequest(response); // Trả về 400 nếu có lỗi
            }
        }




        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files.FirstOrDefault(); // Lấy tệp từ request
            var maKhachTro = Request.Form["maKhachTro"]; // Lấy mã khách trọ từ request
            var hoTen = Request.Form["hoTen"]; // Lấy họ tên từ request
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\CongViecHocTap\\TailieuCNTT\\Môn học\\Đồ án chuyên ngành\\Source đồ án\\Web Phòng Trọ\\Apartment_Management_Web\\Apartment_Management_Web_GUI\\wwwroot\\images"); // Thư mục lưu hình ảnh

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Không có tệp nào được tải lên." });
            }

            try
            {
                // Kiểm tra thư mục có tồn tại hay không, nếu không thì tạo
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Xử lý họ tên để loại bỏ dấu và khoảng trắng
                var tenKhongDau = RemoveDiacritics(hoTen).Replace(" ", "");

                // Đặt tên file theo định dạng CK_MaKhachTro_HoTen.jpg
                var fileName = $"CK_{maKhachTro}_{tenKhongDau}.jpg";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Lưu tệp vào thư mục
                }

                // Ghi tên file vào database
                await _ThongTinKhachService.UpdateChuKyAsync(maKhachTro, fileName);

                return Ok(new { message = "Upload thành công!", fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Upload thất bại!", error = ex.Message });
            }
        }

        // Phương thức để loại bỏ dấu trong chuỗi
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }



      
     


    }
}
