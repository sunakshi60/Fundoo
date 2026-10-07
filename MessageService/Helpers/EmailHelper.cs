
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _configuration;

        public EmailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var emailSettings =
                _configuration.GetSection("EmailSettings");

            var senderEmail = emailSettings["Email"];
            var password = emailSettings["Password"];

            var mail = new MailMessage();

            mail.From = new MailAddress(senderEmail);
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using var smtp = new SmtpClient(
                "smtp.gmail.com",
                587);

            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential(
                senderEmail,
                password);

            await smtp.SendMailAsync(mail);
        }
    }
}