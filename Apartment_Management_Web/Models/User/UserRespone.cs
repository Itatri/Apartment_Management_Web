namespace Apartment_Management_Web.Models.User
{
    public class UserRespone
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserPhong? User { get; set; }

    }

    public class UpdateUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserPhong UpdatedUser { get; set; }
    }

}
