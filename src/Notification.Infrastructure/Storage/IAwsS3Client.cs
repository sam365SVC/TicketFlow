using System;
using System.Collections.Generic;
using System.Text;
using TicketFlow.Shared.Events;

namespace Notification.Infrastructure.Storage
{
    public interface IAwsS3Client
    {
        /// <summary>
        /// Sube un archivo PDF de un ticket a Amazon S3 y devuelve la URL pública.
        /// </summary>
        /// <param name="ticketId">El identificador único del ticket (ej. TICKET-123)</param>
        /// <param name="fileBytes">El contenido del archivo PDF en bytes</param>
        /// <param name="cancellationToken">Token para cancelar la operación si es necesario</param>
        /// <returns>La URL pública donde quedó alojado el archivo</returns>
        Task<string> UploadTicketAsync(string objectKey,DateTime expirationDate, byte[] fileBytes, CancellationToken cancellationToken = default);
    }
}
