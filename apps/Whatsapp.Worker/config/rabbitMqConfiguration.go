package config

import "os"

type RabbitConfig struct {
	RabbitMqUrl string
}

func LoadConfig() *RabbitConfig {
	rabbitURL := os.Getenv("RABBITMQ_URL")
	if rabbitURL == "" {
		rabbitURL = "amqp://guest:guest@localhost:5672/"
	}
	return &RabbitConfig{
		RabbitMqUrl: rabbitURL,
	}
}