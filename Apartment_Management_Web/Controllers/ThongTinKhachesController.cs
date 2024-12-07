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

        // API lấy danh sách thành viên phòng 
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
                return NotFound(response);
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin khách thành công.";
            response.Khach = thongTinKhach;

            return Ok(response);
        }

        // API lấy thông tin khách theo phòng
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
                return NotFound(response);
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin khách thành công.";
            response.Khachs = thongTinKhach;

            return Ok(response);
        }



        // API lấy thông tin thanh viên theo mã thành viên
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
                return NotFound(response);
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin khách thành công.";
            response.Khachs = thongTinKhach;

            return Ok(response);
        }


        // API cập nhật thông tin thành viên
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
                Khachs = null
            });
        }

        // API kê khai thông tin thành viên phòng
        [HttpPost("CreateThongTinKhach")]
        [Authorize]
        public async Task<ActionResult<APICustomerRespone>> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var response = new APICustomerRespone();

            try
            {
                // Tạo ID thành viên
                var lastCustomer = await _ThongTinKhachService.GetLastCustomerAsync();
                int nextNumber = 1;

                if (lastCustomer != null)
                {

                    var lastNumber = int.Parse(lastCustomer.MaKhachTro.Substring(2));
                    nextNumber = lastNumber + 1;
                }


                string maKhachTro = "KH" + nextNumber.ToString("D3");


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
                    ChuKy = request.ChuKy,
                    ThuongTru = request.ThuongTru,
                    TrangThai = 1
                };


                var createdCustomer = await _ThongTinKhachService.CreateCustomerAsync(newCustomer);

                response.IsSuccess = true;
                response.Message = "Tạo khách hàng mới thành công.";
                response.Khachs = new List<ThongTinKhach> { createdCustomer };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Đã xảy ra lỗi khi tạo khách hàng mới: " + ex.Message;
                return BadRequest(response);
            }
        }

        // API kiểm tra phòng đã có chủ hộ chưa
        [Authorize]
        [HttpGet("CheckRoomHasOwner")]
        public async Task<ActionResult<APICustomerRespone>> CheckRoomHasOwner(string maPhong)
        {

            var thongTinKhach = await _ThongTinKhachService.GetThongTinKhachByPhongAsync(maPhong);

            var response = new APICustomerRespone();


            if (thongTinKhach == null || thongTinKhach.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = "Phòng chưa có chủ hộ.";
                response.Khachs = null;
                return Ok(response);
            }


            var hasOwner = thongTinKhach.Any(khach => khach.QuanHe == "Chủ hộ");

            if (hasOwner)
            {

                response.IsSuccess = false;
                response.Message = "Phòng này đã có chủ hộ. Không thể thêm chủ hộ mới.";
                response.Khachs = thongTinKhach;
                return Ok(response);
            }

            response.IsSuccess = true;
            response.Message = "Phòng chưa có chủ hộ.";
            response.Khachs = thongTinKhach;

            return Ok(response);
        }






        // API Upload hình ảnh chữ ký thành viên phòng
        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files.FirstOrDefault();
            var maKhachTro = Request.Form["maKhachTro"];
            var hoTen = Request.Form["hoTen"];

            try
            {

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
