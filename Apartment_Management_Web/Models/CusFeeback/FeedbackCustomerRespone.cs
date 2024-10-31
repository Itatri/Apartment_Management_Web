namespace Apartment_Management_Web.Models.CusFeeback
{
    public class FeedbackCustomerRespone
    {
        public bool IsSuccess { get; set; } // Trạng thái lấy dữ liệu thành công hay không
        public string Message { get; set; } // Thông điệp về tình trạng
        public List<FeedBack>? FeedBacks { get; set; } // Danh sách phiếu thu (nếu có)
        public int TotalCount { get; set; } // Tổng số bản ghi
        public int TotalPages { get; set; } // Tổng số trang
    }
}
