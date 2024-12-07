using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.Bill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web.Controllers
{
    [Route("api/PhieuThus")]
    [ApiController]
    public class PhieuThusController : ControllerBase
    {
        private readonly IPhieuThuService _PhieuThuService;

        public PhieuThusController(IPhieuThuService ThongTinPhieuThuService)
        {
            _PhieuThuService = ThongTinPhieuThuService;
        }


        // API lấy danh sách phiếu thu của phòng
        [Authorize]
        [HttpGet("GetAllThongTinPhieuThu")]
        public async Task<ActionResult<IEnumerable<PhieuThu>>> GetPhieuThus()
        {
            var PhieuThu = await _PhieuThuService.GetAllThongTinPhieuThuAsync();
            return Ok(PhieuThu);
        }


        // API lấy danh sách phiếu thu theo phòng
        [Authorize]
        [HttpGet("GetThongTinPhieuThuBy_MaPhong")]
        public async Task<ActionResult<BillCustomerRespone>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong, DateOnly? startDate, DateOnly? endDate, bool? trangThai, int pageNumber = 1, int pageSize = 5)
        {

            var thongtinPhieuThu = await _PhieuThuService.GetThongTinPhieuThuBy_MaPhongAsync(maPhong, startDate, endDate, trangThai, pageNumber, pageSize);


            var totalCount = await _PhieuThuService.GetTotalCountAsync(maPhong, startDate, endDate, trangThai);

            var response = new BillCustomerRespone();

            if (thongtinPhieuThu == null || !thongtinPhieuThu.Any())
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thông tin phiếu thu.";
                response.Phieuthus = null;
                response.TotalCount = totalCount;
                response.TotalPages = 0;
                return NotFound(response);
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin phiếu thu thành công.";
            response.Phieuthus = thongtinPhieuThu;
            response.TotalCount = totalCount;
            response.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(response);
        }

        // API cập nhật điện nước mới của phiếu thu 
        [Authorize]
        [HttpPut("UpdatePhieuThu")]
        public async Task<ActionResult<BillCustomerRespone>> UpdatePhieuThuAsync(string maPt, decimal dienMoi, decimal nuocMoi)
        {

            var phieuThu = await _PhieuThuService.GetPhieuThuByMaPtAsync(maPt);

            var response = new BillCustomerRespone();

            if (phieuThu == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy phiếu thu với mã phiếu thu.";
                return NotFound(response);
            }


            phieuThu.DienMoi = (double?)dienMoi;
            phieuThu.NuocMoi = (double?)nuocMoi;


            var updateResult = await _PhieuThuService.UpdatePhieuThuAsync(phieuThu);

            if (!updateResult)
            {
                response.IsSuccess = false;
                response.Message = "Cập nhật phiếu thu không thành công.";
                return StatusCode(500, response);
            }

            response.IsSuccess = true;
            response.Message = "Cập nhật phiếu thu thành công.";
            response.Phieuthus = new List<PhieuThu> { phieuThu };

            return Ok(response);
        }


        // API xuất phiếu thu thành PDF
        [HttpGet("ExportPhieuThuToPdf")]
        public async Task<IActionResult> ExportPhieuThuToPdf(string maPt)
        {
            try
            {
                var fileBytes = await _PhieuThuService.ExportPhieuThuToPdfAsync(maPt);
                return File(fileBytes, "application/pdf", $"PhieuThu_{maPt}.pdf");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // API lấy thông tin Admin của phòng
        [Authorize]
        [HttpGet("GetAdminInfoByMaPhong")]
        public async Task<ActionResult<AdminInfoResponse>> GetAdminInfoByMaPhong(string maPhong)
        {
            var adminInfo = await _PhieuThuService.GetAdminInfoByMaPhongAsync(maPhong);
            if (adminInfo == null)
            {
                return NotFound(new AdminInfoResponse { IsSuccess = false, Message = "Không tìm thấy thông tin Admin" });
            }
            return Ok(adminInfo);
        }






    }
}
