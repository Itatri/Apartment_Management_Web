﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Apartment_Management_Web.Models;
using Apartment_Management_Web.Services;
using Apartment_Management_Web.Models.Customer;

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
        [HttpGet("GetAllThongTinKhach")]
        public async Task<ActionResult<IEnumerable<ThongTinKhach>>> GetThongTinKhachs()
        {
            var thongtinKhach = await _ThongTinKhachService.GetAllThongTinKhachAsync();
            return Ok(thongtinKhach);
        }

       

        // API lấy thông tin khách theo CCCD và Phone
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


        [HttpPost("Upload")]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files.FirstOrDefault(); // Lấy tệp từ request
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\CongViecHocTap\\TailieuCNTT\\Môn học\\Đồ án chuyên ngành\\Đồ án\\Web Phòng Trọ\\Apartment_Management_Web\\Apartment_Management_Web_GUI\\wwwroot\\images"); // Thư mục lưu hình ảnh

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

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Lưu tệp vào thư mục
                }

                return Ok(new { message = "Upload thành công!", fileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Upload thất bại!", error = ex.Message });
            }
        }





    }
}
