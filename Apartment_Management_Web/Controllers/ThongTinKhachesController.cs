using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



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
                Khachs = null // Trả về thông tin khách đã cập nhật 
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

            try
            {
                // Gọi phương thức UploadChuKyAsync từ service
                var fileName = await _ThongTinKhachService.UploadChuKyAsync(file, maKhachTro, hoTen);
                return Ok(new { message = "Upload thành công!", fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Upload thất bại!", error = ex.Message });
            }
        }

        // API lấy danh sách Quan Hệ
        [HttpGet("GetQuanHe")]
        public ActionResult<IEnumerable<string>> GetQuanHe()
        {
            var quanHeList = new List<string>
            {
                "Chủ hộ", "Ba", "Mẹ", "Bố", "Cha", "Cha chồng", "Cha đẻ", "Cha nuôi",
                "Cha vợ", "Mẹ chồng", "Mẹ đẻ", "Mẹ nuôi", "Mẹ vợ", "Chồng", "Vợ",
                "Con", "Con chồng", "Con dâu", "Con đẻ", "Con nuôi", "Con rể", "Con vợ",
                "Cháu", "Cháu dâu", "Cháu họ", "Cháu ngoại", "Cháu nội", "Cháu rể",
                "Anh", "Anh chồng", "Anh họ", "Anh rể", "Anh ruột", "Anh vợ", "Chị",
                "Chị chồng", "Chị dâu", "Chị họ", "Chị ruột", "Chị vợ", "Em", "Em chồng",
                "Em dâu", "Em họ", "Em rể", "Em ruột", "Em vợ", "Bà", "Bà ngoại", "Bà nội",
                "Ông", "Ông ngoại", "Ông nội", "Cô", "Dì", "Bác", "Thím", "Tía", "Bạn",
                "Người được chăm sóc", "Người được giám hộ", "Người được nuôi dưỡng",
                "Người được trợ giúp", "Người giám hộ", "Người mượn nhà", "Người ở nhờ",
                "Người thuê nhà", "Nhân khẩu tập thể", "Chưa có thông tin", "Cắt", "Cùng ở thuê",
                "Khác"
            };
            return Ok(quanHeList);
        }


    }
}
