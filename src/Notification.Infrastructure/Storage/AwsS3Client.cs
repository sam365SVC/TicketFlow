using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Infrastructure.Configurations;
using TicketFlow.Shared.Events;

namespace Notification.Infrastructure.Storage
{
    public class AwsS3Client(ILogger<AwsS3Client> logger, IOptions<AwsS3Options> options):IAwsS3Client
    {
        private readonly AmazonS3Client _s3Client = new(
            options.Value.AccessKey,
            options.Value.SecretKey,
            RegionEndpoint.GetBySystemName(options.Value.Region)
        );

        public async Task<string> UploadTicketAsync(string objectKey, DateTime expirationDate, byte[] fileBytes, CancellationToken cancellationToken = default)
        {
            // 🚀 SOLUCIÓN A CA1873: Envolver el log en un IF
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("☁️ [AWS S3 REAL] Iniciando subida a S3 para el ticket {TicketId}...",objectKey);
            }

            using var memoryStream = new MemoryStream(fileBytes);

            var putRequest = new PutObjectRequest
            {
                BucketName = options.Value.BucketName, // Accedemos vía .Value
                Key = objectKey,
                InputStream = memoryStream,
                ContentType = "application/pdf",

            };

            await _s3Client.PutObjectAsync(putRequest, cancellationToken);

            var preSignedUrl = new GetPreSignedUrlRequest
            {
                BucketName = options.Value.BucketName,
                Key = objectKey,
                Expires = expirationDate
            };

            string publicUrl = _s3Client.GetPreSignedURL(preSignedUrl);

            // 🚀 SOLUCIÓN A CA1873: Envolver el log en un IF
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("✅ [AWS S3 REAL] ¡Subida exitosa de forma real! URL: {Url}", publicUrl);
            }

            return publicUrl;
        }
    }
}
