using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Models.CusFeeback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web.Controllers
{
    [Route("api/FeedBacks")]
    [ApiController]
    public class FeedBacksController : ControllerBase
    {

        private readonly IFeedbackService _FeedbackService;

        public FeedBacksController(IFeedbackService FeedbackService)
        {
            _FeedbackService = FeedbackService;
        }

        // API lấy danh sách Feedback
        [Authorize]
        [HttpGet("GetAllThongTinFeedback")]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetFeedbacks()
        {
            var Feedback = await _FeedbackService.GetAllThongTinFeedbackAsync();
            return Ok(Feedback);
        }


        // API lấy danh sách Feedback theo mã phòng
        [Authorize]
        [HttpGet("GetThongTinFeedbackBy_MaPhong")]
        public async Task<ActionResult<FeedbackCustomerRespone>> GetThongTinFeedbackBy_MaPhong(string maPhong, DateTime? startDate, DateTime? endDate, int? trangThai, int pageNumber = 1, int pageSize = 100)
        {
            var thongtinFeeback = await _FeedbackService.GetThongTinFeedBacksBy_MaPhongAsync(maPhong, startDate, endDate, trangThai, pageNumber, pageSize);

            var response = new FeedbackCustomerRespone();

            if (thongtinFeeback == null || !thongtinFeeback.Any())
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thông tin phản hồi.";
                response.FeedBacks = null;
                return NotFound(response);
            }


            var totalCount = await _FeedbackService.GetTotalFeedbackCount(maPhong, startDate, endDate, trangThai);
            response.IsSuccess = true;
            response.Message = "Lấy thông tin phản hồi thành công.";
            response.FeedBacks = thongtinFeeback;
            response.TotalCount = totalCount;
            response.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(response);
        }

        // API khách hàng gửi Feedback
        [Authorize]
        [HttpPost("CreateFeedback")]
        public async Task<ActionResult<FeedbackCustomerRespone>> CreateFeedback([FromBody] CreateFeedbackRequest request)
        {
            var response = new FeedbackCustomerRespone();

            try
            {
                // Tạo ID Feedback
                var lastFeedback = await _FeedbackService.GetLastFeedbackAsync();
                int nextNumber = 1;

                if (lastFeedback != null)
                {

                    var lastNumber = int.Parse(lastFeedback.MaFb.Substring(2));
                    nextNumber = lastNumber + 1;
                }


                string maFb = "FB" + nextNumber.ToString("D4");


                var newFeedback = new FeedBack
                {
                    MaFb = maFb,
                    MaPhong = request.MaPhong,
                    MoTa = request.MoTa,
                    NgayGui = DateTime.Now,
                    PhanHoi = null,
                    NgayPhanHoi = null,
                    TrangThai = 0
                };


                var createdFeedback = await _FeedbackService.CreateFeedbackAsync(newFeedback);

                response.IsSuccess = true;
                response.Message = "Tạo phản hồi mới thành công.";
                response.FeedBacks = new List<FeedBack> { createdFeedback };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Đã xảy ra lỗi khi tạo phản hồi mới: " + ex.Message;
                return BadRequest(response);
            }
        }

    }
}
