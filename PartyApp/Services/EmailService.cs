using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PartyInvitationManager.Services
{
    public interface IEmailService
    {
        Task SendInvitationEmailAsync(string recipientEmail, string recipientName, string partyDescription,
            DateTime partyDate, string partyLocation, int invitationId);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendInvitationEmailAsync(string recipientEmail, string recipientName, string partyDescription,
            DateTime partyDate, string partyLocation, int invitationId)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var smtpClient = new SmtpClient(smtpSettings["Host"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
            };

            var message = new MailMessage
            {
                From = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]),
                Subject = $"Invitation to {partyDescription}",
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(recipientEmail, recipientName));

            // Generate response URL
            var baseUrl = _configuration["ApplicationUrl"];
            var responseUrl = $"{baseUrl}/Invitation/Respond/{invitationId}";

            message.Body = $@"
                <html>
                <body>
                    <h2>You're invited to {partyDescription}!</h2>
                    <p>Hello {recipientName},</p>
                    <p>You are cordially invited to {partyDescription} on {partyDate.ToLongDateString()} at {partyLocation}.</p>
                    <p>Please <a href='{responseUrl}'>click here</a> to respond to this invitation.</p>
                    <p>We hope to see you there!</p>
                </body>
                </html>";

            await smtpClient.SendMailAsync(message);
        }
    }
}