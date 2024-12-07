using Apartment_Management_Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web.Controllers
{
    // API gửi Mail hỗ trợ đến khách hàng
    [Route("api/Mails")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSenderService _emailSender;

        public EmailController(IEmailSenderService emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(string toEmail)
        {
            await _emailSender.SendEmailAsync(toEmail, "IT Apartment Support", "Nếu có vấn đề cần hỗ trợ , hãy gửi lại thông tin chi tiết tới Email này chúng tôi . ");
            return Ok("Email đã được gửi !");
        }
    }
}
