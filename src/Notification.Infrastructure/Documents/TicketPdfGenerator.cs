using TicketFlow.Shared.Models;
using TicketFlow.Shared.Events;
using Notification.Infrastructure.Documents.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace Notification.Infrastructure.Documents
{
    public class TicketPdfGenerator:ITicketPdfGenerator
    {
        public byte[] GeneratePdf(ReservationConfirmedEvent eventInfo, TicketInfo ticket, byte[] qrImageBytes)
        {
            string colorZona = ticket.ZoneName?.ToUpper() switch
            {
                "VIP" => Colors.Amber.Medium,
                "BOX" => Colors.Red.Darken1,
                "GENERAL" => Colors.Blue.Darken2,
                _ => Colors.Grey.Darken3
            };
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A6);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);

                    page.Header().Background(colorZona).Padding(10).AlignCenter()
                        .Text($"ACCESO {ticket.ZoneName?.ToUpper()}")
                        .Bold().FontSize(16).FontColor(Colors.White);

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text(eventInfo.EventName).Bold().FontSize(14);
                        col.Item().Text($"Lugar: {eventInfo.Location}");
                        col.Item().Text($"Fecha: {eventInfo.EventDate:dd/MM/yyyy HH:mm}");
                        col.Item().Text($"A nombre de: {eventInfo.CustomerName}").Medium();

                        col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        col.Item().AlignCenter().Text("ESCANEA PARA INGRESAR").FontSize(10).FontColor(Colors.Grey.Darken1);

                        col.Item().AlignCenter().Width(120).Image(qrImageBytes);
                    });

                    page.Footer().AlignCenter().Text($"Ticket: {ticket.TicketCode}").FontSize(8);
                });
            });

            return document.GeneratePdf();
        }
    }
}
