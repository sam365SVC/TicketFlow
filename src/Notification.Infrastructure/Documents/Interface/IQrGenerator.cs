using System;
using System.Collections.Generic;
using System.Text;

namespace Notification.Infrastructure.Documents.Interface
{
    public interface IQrGenerator
    {
        byte[] GenerateValidationQrCode(string ticketCode);
    }
}
