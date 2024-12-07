namespace Apartment_Management_Web.Models.CusFeeback
{
    public class FeedbackCustomerRespone
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<FeedBack>? FeedBacks { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
