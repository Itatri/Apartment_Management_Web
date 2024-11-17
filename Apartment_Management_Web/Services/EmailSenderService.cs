using Apartment_Management_Web.Interfaces;
using Apartment_Management_Web.Models.Mail;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Apartment_Management_Web.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;


        public EmailSenderService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }


        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                // Tạo SmtpClient để gửi email
                var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                // Tạo MailMessage
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    IsBodyHtml = true // Cho phép nội dung HTML
                };

                // Đường dẫn đến logo
                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "mailsupport.jpg");

                // Kiểm tra xem file logo có tồn tại không
                if (!File.Exists(logoPath))
                {
                    throw new FileNotFoundException("Logo file not found at " + logoPath);
                }

                // Tạo nội dung HTML với hình ảnh chèn trong email
                string htmlBody = $@"
                    <html>
                        <body>
                            <h1>Xin chào!</h1>
                            <p>{message}</p>
                            <img src='cid:LogoImage' alt='Logo' style='width:500px; height:auto;' />
                        </body>
                    </html>";

                // Gán nội dung HTML vào email
                var altView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                // Tạo tài nguyên hình ảnh để chèn vào nội dung email
                var logo = new LinkedResource(logoPath)
                {
                    ContentId = "LogoImage", // ID được tham chiếu trong HTML
                    ContentType = new System.Net.Mime.ContentType("image/png")
                };

                // Thêm tài nguyên hình ảnh vào AlternateView
                altView.LinkedResources.Add(logo);

                // Thêm AlternateView vào email
                mailMessage.AlternateViews.Add(altView);

                // Thêm người nhận email
                mailMessage.To.Add(toEmail);

                // Gửi email
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new InvalidOperationException("Sending email failed.", ex);
            }
        }

    }
}
