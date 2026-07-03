using MongoDB.Bson.Serialization;
using Notification.Infrastructure.Mongo.Entities;
namespace Notification.Infrastructure.Mongo.Data.Configurations
{
    public class TicketItemInfoConfiguration:IMongoClassMap
    {
        public void Register()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(TicketItemInfo)))
            {
                return;
            }
            BsonClassMap.RegisterClassMap<TicketItemInfo>(ti =>
            {
                ti.AutoMap();

                ti.SetIgnoreExtraElements(true);

                ti.MapMember(t => t.BookingItemId).SetElementName("bookingItemId");
                ti.MapMember(t => t.TicketCode).SetElementName("ticketCode");
            });
        }
    }
}
