using Apartment_Management_Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Apartment_Management_Web.Controllers
{
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
            await _emailSender.SendEmailAsync(toEmail, "IT Apartment Support", "Xin chào ! Nếu có vấn đề cần hỗ trợ , hãy gửi thông tin chi tiết tới chúng tôi . ");
            return Ok("Email đã được gửi!");
        }
    }
}
