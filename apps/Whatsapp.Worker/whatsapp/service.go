package whatsapp

import (
	"context"
	"fmt"

	"go.mau.fi/whatsmeow"
	"go.mau.fi/whatsmeow/proto/waE2E"
	"go.mau.fi/whatsmeow/types"
	"google.golang.org/protobuf/proto"
)

type ServiceWhatsapp struct {
	client *whatsmeow.Client
}

func NewServiceWhatsapp(c *whatsmeow.Client) *ServiceWhatsapp {
	return &ServiceWhatsapp{
		client: c,
	}
}

func (s ServiceWhatsapp) SendMessage(phone string, textMessage string ) error {
	jid := types.NewJID(phone, types.DefaultUserServer)
	msg := &waE2E.Message{
		Conversation: proto.String(textMessage),
	}

	_, err := s.client.SendMessage(context.Background(), jid, msg)
	if err != nil {
		return fmt.Errorf("error sended message to %s: %w", phone, err)
	}
	return nil
}
