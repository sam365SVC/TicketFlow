using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Notification.Infrastructure.Mongo.Entities;

namespace Notification.Infrastructure.Mongo.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            // 1. 🚀 Escaneo automático de configuraciones Fluent API
            ApplyConfigurations();

            // 2. Validar de forma segura la cadena de conexión
            var connectionString = configuration.GetConnectionString("MongoConnection");
            // 🚀 SOLUCIÓN A CA2208: Usar InvalidOperationException para estados no válidos
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("La cadena de conexión 'MongoConnection' no está configurada en el appsettings o variables de entorno.");
            }

            // 3. Usar MongoUrl de forma centralizada para evitar errores de parseo
            var mongoUrl = new MongoUrl(connectionString);
            var client = new MongoClient(mongoUrl);

            // Si no se define base de datos en la URL, se asigna una por defecto
            var databaseName = mongoUrl.DatabaseName ?? "TicketFlowDb";
            _database = client.GetDatabase(databaseName);

            // 4. Crear los índices de forma segura
            ConfigureIndexes();
        }

        // 📝 Tus colecciones expuestas como propiedades
        public IMongoCollection<NotificationDocument> Notifications =>
            _database.GetCollection<NotificationDocument>("Notifications");

        public IMongoCollection<NotificationTemplate> Templates =>
            _database.GetCollection<NotificationTemplate>("NotificationTemplates");

        private static void ApplyConfigurations()
        {
            var configurationTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IMongoClassMap).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in configurationTypes)
            {
                var configuration = (IMongoClassMap)Activator.CreateInstance(type)!;
                configuration.Register();
            }
        }

        private void ConfigureIndexes()
        {
            try
            {
                // Definición del índice ascendente para BookingId
                var indexKeysDefinition = Builders<NotificationDocument>.IndexKeys.Ascending(n => n.BookingId);
                var indexOptions = new CreateIndexOptions { Unique = true };
                var indexModel = new CreateIndexModel<NotificationDocument>(indexKeysDefinition, indexOptions);

                // Mongo se encarga de verificar si ya existe; si no, lo crea de fondo
                Notifications.Indexes.CreateOne(indexModel);
            }
            catch (Exception ex)
            {
                // Evita que un error creando índices tire abajo el arranque del Worker, pero deja rastro en logs
                Console.WriteLine($"[Error] No se pudo verificar/crear los índices en MongoDB: {ex.Message}");
            }
        }
    }
}