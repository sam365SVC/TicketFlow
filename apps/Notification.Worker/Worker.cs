using Notification.Infrastructure.Mongo.Data;

namespace Notification.Worker
{
    public class Worker(ILogger<Worker> logger, MongoDbContext mongoDb) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var collectionName = mongoDb.Notifications.CollectionNamespace.CollectionName;

                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("✅ Connection successful. MongoDB database initialized and ready. Checking collection: {CollectionName}", collectionName);
                }
                while (!stoppingToken.IsCancellationRequested)
                {
                    // Aquí en el futuro escucharemos los eventos de RabbitMQ...
                    await Task.Delay(5000, stoppingToken);
                }
            }
            // 🔥 NUEVO: Atrapamos la cancelación normal del sistema y la ignoramos
            catch (TaskCanceledException)
            {
                logger.LogInformation("⚠️ El worker se está deteniendo porque el sistema solicitó un apagado.");
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "❌ Fatal error connecting to the database or dependencies.");
            }
        }
    }
}