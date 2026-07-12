package rabbitmq

import (
	"encoding/json"
	"fmt"
	"log"
	"ticketflow/whatsapp-worker/event"
	"ticketflow/whatsapp-worker/message"
	"ticketflow/whatsapp-worker/whatsapp"

	"github.com/rabbitmq/amqp091-go"
)

func StartConsumer(waService *whatsapp.ServiceWhatsapp, rabbitUrl string) {
	conn, err := amqp091.Dial(rabbitUrl)
	if err != nil {
		log.Fatalf("❌ Error open RabbitMq chanel: %s", err)
	}
	defer conn.Close()
	log.Println("✅ Conection RabbitMq sucess")

	ch, err := conn.Channel()
	if err != nil {
		log.Fatalf("❌ Error opening RabbitMq chanel: %s", err)
	}
	defer ch.Close()
	q, err := ch.QueueDeclare(
		"whatsapp_notification_queue",
		true, false, false, false, nil,
	)
	if err != nil {
		log.Fatalf("❌ Error declare queue: %s", err)
	}

	msgs, err := ch.Consume(
		q.Name, "whatsapp_worker",
		true, false, false, false, nil,
	)
	if err != nil {
		log.Fatalf("❌ Error registring the consumer: %s", err)
	}
	fmt.Println("🎧 [*] Worker Go listening RabbitMq events ")

	for d := range msgs {
		var evt event.ReservationConfirmedEvent

		if err := json.Unmarshal(d.Body, &evt); err != nil {
			log.Printf("⚠️ Error JSON decodificatio: %s", err)
			continue
		}
		fmt.Printf("✅ Event received for: %s", evt.CustomerName)

		textMessage := message.MessageSendTickets(evt)

		err := waService.SendMessage(evt.CustomerPhone, textMessage)
		if err != nil {
			log.Printf("❌Error send message for Whatsapp: %s", err)
		} else {
			log.Printf("💬 Whatsapp send success to %s", evt.CustomerPhone)
		}
	}
}
