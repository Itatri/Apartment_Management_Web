namespace Apartment_Management_Web.Models.Customer
{
    public class UpdateThongTinKhachRequest
    {
        public string? HoTen { get; set; }
        public string? GioiTinh { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string? Cccd { get; set; }
        public DateOnly? NgayCap { get; set; }
        public string? NoiCap { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? QueQuan { get; set; }
        public string? ThuongTru { get; set; }
        public string? QuanHe { get; set; }
        public string? ChuKy { get; set; }
        public string? MaPhong { get; set; }
        public int? TrangThai { get; set; }
    }


    public class CreateCustomerRequest
    {
        public string HoTen { get; set; }
        public string GioiTinh { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string Cccd { get; set; }
        public DateOnly? NgayCap { get; set; }
        public string NoiCap { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string QueQuan { get; set; }
        public string QuanHe { get; set; }
        public string MaPhong { get; set; }
        public string ChuKy { get; set; } // Thêm trường chữ ký
        public string? ThuongTru { get; set; }

    }


}
