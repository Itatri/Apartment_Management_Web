namespace Apartment_Management_Web.Models.Customer
{
    public class APICustomerRespone
    {
        public bool IsSuccess { get; set; } // Trạng thái đăng nhập thành công hay không
        public string Message { get; set; }   // Thông điệp về tình trạng
        public ThongTinKhach Khach { get; set; }   // Thông tin người dùng (nếu cần)
        public List<ThongTinKhach>? Khachs { get; set; }   // Danh sách thông tin người dùng (nếu cần)

    }


}
