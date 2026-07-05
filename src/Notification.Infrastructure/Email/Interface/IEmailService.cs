using Notification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Email.Interface
{
    public interface IEmailService
    {
        Task SendTicketEmailAsync(string toEmail, ReservationEmailModel model);
    }
}
