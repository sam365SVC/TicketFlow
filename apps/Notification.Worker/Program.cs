
using Microsoft.Extensions.Options;
using Notification.Domain.Events;
using Notification.Infrastructure.Configurations;
using Notification.Infrastructure.Consumers;
using Notification.Infrastructure.Documents;
using Notification.Infrastructure.Documents.Interface;
using Notification.Infrastructure.Email;
using Notification.Infrastructure.Email.Interface;
using Notification.Infrastructure.Mongo.Data;
using Notification.Infrastructure.Storage;
using Notification.Worker;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<AwsS3Options>(builder.Configuration.GetSection(AwsS3Options.SectionName));
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.SectionName));

builder.Services.AutoRegisterHandlersFromAssemblyOf<ReservationConfirmedConsumer>();

builder.Services.AddRebus((configure, provider) =>
{
    var rabbitOptions = provider.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
    var connectionString = $"amqp://{rabbitOptions.Username}:{rabbitOptions.Password}@{rabbitOptions.Host}:{rabbitOptions.Port}";

    return configure
        .Logging(l => l.MicrosoftExtensionsLogging(provider.GetRequiredService<ILoggerFactory>()))
        // Configuramos el transporte (RabbitMQ) y el nombre de la cola para este Worker
        .Transport(t => t.UseRabbitMq(connectionString, "ticketflow-notifications-queue"))
        // Opcional pero recomendado: Le decimos a Rebus cómo enrutar este tipo de evento
        .Routing(r => r.TypeBased().MapAssemblyOf<ReservationConfirmedEvent>("ticketflow-notifications-queue"));
});

builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddTransient<IAwsS3Client, AwsS3Client>();
builder.Services.AddTransient<IQrGenerator, QrGenerator>();
builder.Services.AddTransient<ITicketPdfGenerator, TicketPdfGenerator>();
builder.Services.AddTransient<ITicketOrchestrator, TicketOrchestrator>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddHostedService<Worker>();
builder.Services.AddSingleton<RazorEngineCore.IRazorEngine, RazorEngineCore.RazorEngine>();

var host = builder.Build();
host.Run();
