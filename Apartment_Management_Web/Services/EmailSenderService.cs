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

        // Hàm gửi Mail hỗ trợ đến khách hàng
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {

                var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };


                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    IsBodyHtml = true
                };


                string logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "mailsupport.jpg");


                if (!File.Exists(logoPath))
                {
                    throw new FileNotFoundException("Logo file not found at " + logoPath);
                }


                string htmlBody = $@"
                    <html>
                        <body>
                            <h1>Xin chào!</h1>
                            <p>{message}</p>
                            <img src='cid:LogoImage' alt='Logo' style='width:500px; height:auto;' />
                        </body>
                    </html>";


                var altView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");


                var logo = new LinkedResource(logoPath)
                {
                    ContentId = "LogoImage",
                    ContentType = new System.Net.Mime.ContentType("image/png")
                };


                altView.LinkedResources.Add(logo);


                mailMessage.AlternateViews.Add(altView);


                mailMessage.To.Add(toEmail);


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
