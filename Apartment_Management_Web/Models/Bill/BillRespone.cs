namespace Apartment_Management_Web.Models.Bill
{
    public class BillCustomerRespone
    {
        public bool IsSuccess { get; set; } // Trạng thái lấy dữ liệu thành công hay không
        public string Message { get; set; } // Thông điệp về tình trạng
        public List<PhieuThu>? Phieuthus { get; set; } // Danh sách phiếu thu (nếu có)


    }
}
