using JoygameProject.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
namespace JoygameProject.Infrastructure.Concretes.Services
{
    public class MailService(IConfiguration config) : IMailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = config.GetSection("SmtpSettings");

            using var client = new SmtpClient(smtp["Host"], int.Parse(smtp["Port"]!))
            {
                Credentials = new NetworkCredential(smtp["Email"], smtp["Password"]),
                EnableSsl = bool.Parse(smtp["EnableSsl"]!)
            };

            var mail = new MailMessage
            {
                From = new MailAddress(smtp["Email"]!),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);
        }
    }
}
