using MongoDB.Bson.Serialization;
using Notification.Infrastructure.Mongo.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Mongo.Data.Configurations
{
    public class NotificationTemplateConfiguration:IMongoClassMap
    {
        public void Register()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(NotificationTemplate)))
            {
                return;
            }

            BsonClassMap.RegisterClassMap<NotificationTemplate>(nt =>
            {
                nt.AutoMap();
                nt.SetIgnoreExtraElements(true);
                nt.MapIdMember(nt => nt.Id);

                nt.MapMember(nt => nt.Name).SetElementName("name");
                nt.MapMember(nt => nt.SubjectTemplate).SetElementName("subjectTemplate");
                nt.MapMember(nt => nt.BodyTemplate).SetElementName("bodyTemplate");
                nt.MapMember(nt => nt.IsActive).SetElementName("isActive");
                nt.MapMember(nt => nt.UpdateAt).SetElementName("updateAt");
            });
        }
    }
}
