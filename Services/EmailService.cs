using System.Net;
using System.Net.Mail;

namespace BTGPactualAPI.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmailAddress;

        public EmailService(string fromEmailAddress, string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
        {
            _fromEmailAddress = fromEmailAddress;
            _smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MailMessage(_fromEmailAddress, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await _smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
            }
        }

    }
}
