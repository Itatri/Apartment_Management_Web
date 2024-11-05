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
                var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Ghi lại thông báo lỗi để kiểm tra
                Console.WriteLine($"Error: {ex.Message}");

                // Xử lý ngoại lệ nếu có lỗi
                throw new InvalidOperationException("Sending email failed.", ex);
            }
        }
    }
}
