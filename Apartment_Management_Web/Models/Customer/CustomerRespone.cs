namespace Apartment_Management_Web.Models.Customer
{
    public class APICustomerRespone
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public ThongTinKhach Khach { get; set; }
        public List<ThongTinKhach>? Khachs { get; set; }

    }


}
