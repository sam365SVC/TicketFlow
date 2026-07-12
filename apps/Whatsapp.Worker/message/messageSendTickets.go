package message

import (
	"fmt"
	"strings"
	"ticketflow/whatsapp-worker/event"
)

func MessageSendTickets(data event.ReservationConfirmedEvent) string {
	var builder strings.Builder

	builder.WriteString(fmt.Sprintf("¡Hola *%s* 🎉🎉\n\n", data.CustomerName))
	builder.WriteString("Gracias por su compra. Aquí tienes los detalles de evento: \n\n")
	builder.WriteString(fmt.Sprintf("🎫 *Evento* %s\n", data.EventName))
	builder.WriteString(fmt.Sprintf("🚏 *Ubicación*: %s\n\n", data.Location))
	builder.WriteString("Tus entradas:\n")

	for i, ticket := range data.Tickets {
		builder.WriteString(fmt.Sprintf("%d. *%s*: %s \n", i+1, ticket.ZoneName, ticket.TicketUrl))
	}

	builder.WriteString("\n¡Que disfrutes el evento! 🥳")

	return builder.String()
}
