using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Domain.Models;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Email.Interface;

using RazorEngineCore;

namespace Notification.Infrastructure.Email
{
    public class EmailService(
        IOptions<EmailOptions> options,
        ILogger<EmailService> logger,
        IRazorEngine razorEngine
    ): IEmailService
    {
        public async Task SendTicketEmailAsync(string toEmail, ReservationEmailModel model)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "TicketTemplate.cshtml");
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"No se encontro la plantilla en: {templatePath}");
            }

            string templateContent = await File.ReadAllTextAsync(templatePath);

            var template = await razorEngine.CompileAsync(templateContent);
            string htmlBody = await template.RunAsync(model);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TicketFlow Oficial",options.Value.Email));
            message.To.Add(new MailboxAddress(model.CustomerName, toEmail));
            message.Subject = $"¡Entradas confirmadas para {model.EventName}! 🎉";

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };

            message.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(options.Value.Email, options.Value.PasswordApp);

            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ ¡Correo enviado exitosamente a {Email}!", toEmail);
            }
        }
    }
}
