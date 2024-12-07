namespace Apartment_Management_Web.Models.Bill
{

    public class BillCustomerRespone
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<PhieuThu>? Phieuthus { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
    public class AdminInfoDto
    {
        public string MaAdmin { get; set; }
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { get; set; }
        public string Phone { get; set; }
        public string ChuKy { get; set; }
        public string NganHang { get; set; }
        public string TaiKhoan { get; set; }
        public string IdUser { get; set; }
    }

    public class AdminInfoResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AdminInfoDto AdminInfo { get; set; }
    }


}
