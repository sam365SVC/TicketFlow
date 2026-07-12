package main

import (
	"context"
	"fmt"
	"os"
	"ticketflow/whatsapp-worker/config"
	rabbitmq "ticketflow/whatsapp-worker/rabbitMq"
	"ticketflow/whatsapp-worker/whatsapp"

	_ "github.com/mattn/go-sqlite3"

	"github.com/mdp/qrterminal/v3"

	"go.mau.fi/whatsmeow"
	"go.mau.fi/whatsmeow/store/sqlstore"
	"go.mau.fi/whatsmeow/types/events"
	waLog "go.mau.fi/whatsmeow/util/log"
)

func eventHandler(evt interface{}) {
	switch v := evt.(type) {
	case *events.Message:
		fmt.Printf("📩 Mensaje recibido de %s: %s\n", v.Info.Sender.User, v.Message.GetConversation())
	}
}

func main() {
	dblog := waLog.Stdout("Database", "DEBUG", true)
	clientLog := waLog.Stdout("Client", "DEBUG", true)
	// Creamos el archivo dentro de una carpeta dedicada llamada 'data'
	container, err := sqlstore.New(context.Background(), "sqlite3", "file:data/whatsapp_session.db?_foreign_keys=on", dblog)

	if err != nil {
		panic(err)
	}
	deviceStore, err := container.GetFirstDevice(context.Background())
	if err != nil {
		panic(err)
	}

	client := whatsmeow.NewClient(deviceStore, clientLog)
	client.AddEventHandler(eventHandler)

	if client.Store.ID == nil {
		qrChan, _ := client.GetQRChannel(context.Background())
		err := client.Connect()
		if err != nil {
			panic(err)
		}
		for evt := range qrChan {
			switch evt.Event {
			case "code":
				fmt.Println("\n\t👇 ESCANEA ESTE CÓDIGO QR CON TU WHATSAPP 👇")
				fmt.Println("-------------------------------------------------")
				qrterminal.GenerateHalfBlock(evt.Code, qrterminal.L, os.Stdout)
				fmt.Println("-------------------------------------------------")
			case "timeout":
				fmt.Println("\n⏳ El código QR caducó. Generando uno nuevo...")
			case "success":
				fmt.Println("\n✅ ¡VINCULACIÓN EXITOSA CON EL TELÉFONO!")
			}
		}
	} else {
		err := client.Connect()
		if err != nil {
			panic(err)
		}
	}
	waService := whatsapp.NewServiceWhatsapp(client)

	cfg := config.LoadConfig()

	go rabbitmq.StartConsumer(waService, cfg.RabbitMqUrl)

	select {}
}
