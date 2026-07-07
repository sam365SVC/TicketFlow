using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Notification.Domain.Models;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Email.Interface;
using Scriban;
using Scriban.Runtime;

namespace Notification.Infrastructure.Email
{
    public class EmailService(
        IOptions<EmailOptions> options,
        ILogger<EmailService> logger
 
    ) : IEmailService
    {
        public async Task SendTicketEmailAsync(string toEmail, ReservationEmailModel model)
        {
            

            var templatePath = Path.Combine(AppContext.BaseDirectory, "Templates", "TicketTemplate.html");
            var templateText = await File.ReadAllTextAsync(templatePath);

            // 2. Parseamos el texto con Scriban
            var template = Template.Parse(templateText);

            // 3. Configuramos el contexto para que conecte C# (PascalCase) con HTML (snake_case)
            var context = new TemplateContext();
            context.MemberRenamer = member => member.Name.ToLower();

            var scriptObject = new ScriptObject();
            scriptObject.Add("model", model);
            context.PushGlobal(scriptObject);

            // 4. Renderizamos la plantilla para obtener el string final
            var htmlResult = await template.RenderAsync(context);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TicketFlow Oficial",options.Value.Email));
            message.To.Add(new MailboxAddress(model.CustomerName, toEmail));
            message.Subject = $"¡Entradas confirmadas para {model.EventName}! 🎉";

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlResult };

            message.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(options.Value.Email, options.Value.PasswordApp);

            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ ¡Correo enviado exitosamente a {Email}!", toEmail);
                logger.LogInformation("La fecha obtenida es: {fecha}", model.EventDate);
            }
        }
    }
}
