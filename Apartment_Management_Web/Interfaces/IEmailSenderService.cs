namespace Apartment_Management_Web.Interfaces
{
    public interface IEmailSenderService
    {
        // Interface API gửi Mail hỗ trợ đến khách hàng
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
