using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Infrastructure.Configurations;

namespace Notification.Infrastructure.Storage
{
    public class AwsS3Client(ILogger<AwsS3Client> logger, IOptions<AwsS3Options> options):IAwsS3Client
    {
        private readonly AmazonS3Client _s3Client = new(
            options.Value.AccessKey,
            options.Value.SecretKey,
            RegionEndpoint.GetBySystemName(options.Value.Region)
        );

        public async Task<string> UploadTicketAsync(string ticketId, byte[] fileBytes, CancellationToken cancellationToken = default)
        {
            // 🚀 SOLUCIÓN A CA1873: Envolver el log en un IF
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("☁️ [AWS S3 REAL] Iniciando subida a S3 para el ticket {TicketId}...", ticketId);
            }

            using var memoryStream = new MemoryStream(fileBytes);

            var putRequest = new PutObjectRequest
            {
                BucketName = options.Value.BucketName, // Accedemos vía .Value
                Key = $"tickets/{ticketId}.pdf",
                InputStream = memoryStream,
                ContentType = "application/pdf"
            };

            await _s3Client.PutObjectAsync(putRequest, cancellationToken);

            string publicUrl = $"https://{options.Value.BucketName}.s3.amazonaws.com/tickets/{ticketId}.pdf";

            // 🚀 SOLUCIÓN A CA1873: Envolver el log en un IF
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ [AWS S3 REAL] ¡Subida exitosa de forma real! URL: {Url}", publicUrl);
            }

            return publicUrl;
        }
    }
}
