package rabbitmq

import (
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	amqp "github.com/rabbitmq/amqp091-go"
)

type RabbitMQExchangeOptions struct {
	Type       string
	Name       string
	Durable    bool
	AutoDelete bool
}

type RabbitMQQueueOptions struct {
	Exclusive  bool
	Durable    bool
	AutoDelete bool
	Name       string
}

func NewRabbitMQ(cfg *config.Config) (*amqp.Connection, error) {
	connection, err := amqp.Dial(cfg.RabbitMQUri)
	if err != nil {
		return nil, err
	}

	return connection, nil
}
