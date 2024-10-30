﻿namespace Apartment_Management_Web.Models.Bill
{
    //public class BillCustomerRespone
    //{
    //    public bool IsSuccess { get; set; } // Trạng thái lấy dữ liệu thành công hay không
    //    public string Message { get; set; } // Thông điệp về tình trạng
    //    public List<PhieuThu>? Phieuthus { get; set; } // Danh sách phiếu thu (nếu có)


    //}


    public class BillCustomerRespone
    {
        public bool IsSuccess { get; set; } // Trạng thái lấy dữ liệu thành công hay không
        public string Message { get; set; } // Thông điệp về tình trạng
        public List<PhieuThu>? Phieuthus { get; set; } // Danh sách phiếu thu (nếu có)
        public int TotalCount { get; set; } // Tổng số bản ghi
        public int TotalPages { get; set; } // Tổng số trang
    }

}
