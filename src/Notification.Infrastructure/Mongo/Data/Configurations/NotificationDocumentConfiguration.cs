using MongoDB.Bson.Serialization;
using Notification.Infrastructure.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Mongo.Data.Configurations
{
    public class NotificationDocumentConfiguration:IMongoClassMap
    {
        public void Register()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(NotificationDocument)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<NotificationDocument>(nd =>
            {
                nd.AutoMap();
                nd.SetIgnoreExtraElements(true);
                nd.MapIdMember(nd => nd.Id);

                nd.MapMember(n => n.BookingId).SetElementName("bookingId");
                nd.MapMember(n => n.UserId).SetElementName("userId");
                nd.MapMember(n => n.NotificationType).SetElementName("notificationType");
                nd.MapMember(n => n.Recipient).SetElementName("recipient");
                nd.MapMember(n => n.Subject).SetElementName("subject");
                nd.MapMember(n => n.Body).SetElementName("body");
                nd.MapMember(n => n.IsPaymentAutorized).SetElementName("isPaymentAuthorized");
                nd.MapMember(n => n.ExternalTransactionId).SetElementName("externalTransactionId");
                nd.MapMember(n => n.GeneratedTickets).SetElementName("generatedTickets");
                nd.MapMember(n => n.TicketFileUrl).SetElementName("ticketFileUrl");
                nd.MapMember(n => n.Status).SetElementName("status");
                nd.MapMember(n => n.Attempts).SetElementName("attempts");
                nd.MapMember(n => n.ErrorMessage).SetElementName("errorMessage");
                nd.MapMember(n => n.CreatedAt).SetElementName("createdAt");
                nd.MapMember(n => n.SentAt).SetElementName("sentAt");
                nd.MapMember(n => n.TemplateCode).SetElementName("templateCode");
            });
        }
    }
}
