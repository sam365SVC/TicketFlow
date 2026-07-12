using System;
using System.Collections.Generic;
using System.Text;
using Notification.Domain.Models;

namespace Notification.Infrastructure.Messaging.Interface
{
    public interface IMessageBusService
    {
        Task PublishWhatsappNotification(ReservationWhatsappModel notification);
    }
}
