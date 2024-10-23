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
using Apartment_Management_Web.Models.Bill;
using Microsoft.AspNetCore.Authorization;


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

        // API lấy danh sách User phòng 
        [HttpGet("GetAllThongTinPhieuThu")]
        public async Task<ActionResult<IEnumerable<PhieuThu>>> GetPhieuThus()
        {
            var PhieuThu = await _PhieuThuService.GetAllThongTinPhieuThuAsync();
            return Ok(PhieuThu);
        }

     

        // API lấy thông tin phiếu thu theo mã phòng
        [HttpGet("GetThongTinPhieuThuBy_MaPhong")]
        public async Task<ActionResult<BillCustomerRespone>> GetThongTinPhieuThuBy_MaPhongAsync(string maPhong)
        {
            var thongtinPhieuThu = await _PhieuThuService.GetThongTinPhieuThuBy_MaPhongAsync(maPhong);

            var response = new BillCustomerRespone();

            if (thongtinPhieuThu == null || !thongtinPhieuThu.Any())
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy thông tin phiếu thu.";
                response.Phieuthus = null; // Chỉnh lại để trả về danh sách
                return NotFound(response); // Trả về trạng thái 404 với response
            }

            response.IsSuccess = true;
            response.Message = "Lấy thông tin phiếu thu thành công.";
            response.Phieuthus = thongtinPhieuThu; // Trả về danh sách phiếu thu tìm thấy

            return Ok(response); // Trả về trạng thái 200 với response
        }

        [Authorize]
        [HttpPut("UpdatePhieuThu")]
        public async Task<ActionResult<BillCustomerRespone>> UpdatePhieuThuAsync(string maPt, decimal dienMoi, decimal nuocMoi)
        {
            // Tìm phiếu thu theo MaPt
            var phieuThu = await _PhieuThuService.GetPhieuThuByMaPtAsync(maPt);

            var response = new BillCustomerRespone();

            if (phieuThu == null)
            {
                response.IsSuccess = false;
                response.Message = "Không tìm thấy phiếu thu với mã phiếu thu.";
                return NotFound(response); // Trả về trạng thái 404 với response
            }

            // Cập nhật thông tin phiếu thu, chuyển đổi từ decimal sang double?
            phieuThu.DienMoi = (double?)dienMoi;
            phieuThu.NuocMoi = (double?)nuocMoi;

            // Lưu thay đổi vào cơ sở dữ liệu
            var updateResult = await _PhieuThuService.UpdatePhieuThuAsync(phieuThu);

            if (!updateResult)
            {
                response.IsSuccess = false;
                response.Message = "Cập nhật phiếu thu không thành công.";
                return StatusCode(500, response); // Trả về trạng thái 500 nếu có lỗi trong quá trình cập nhật
            }

            response.IsSuccess = true;
            response.Message = "Cập nhật phiếu thu thành công.";
            response.Phieuthus = new List<PhieuThu> { phieuThu }; // Trả về thông tin phiếu thu đã cập nhật

            return Ok(response); // Trả về trạng thái 200 với response
        }



    }
}