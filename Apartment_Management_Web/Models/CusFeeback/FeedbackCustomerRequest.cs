namespace Apartment_Management_Web.Models.CusFeeback
{
    public class CreateFeedbackRequest
    {
        public string MaPhong { get; set; } // Mã phòng cần truyền từ phía client
        public string MoTa { get; set; } // Mô tả phản hồi
    }

}
