package rabbitmq

import (
	"fmt"
	"reflect"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus"
	amqp "github.com/rabbitmq/amqp091-go"
)

type RabbitMQEventConsumer struct {
	Connection     *amqp.Connection
	ConsumeOptions map[string]RabbitMQConsumeEventOptions
	EventHandlers  map[string]eventbus.EventHandler
}

type RabbitMQConsumeEventOptions struct {
	ExchangeOptions RabbitMQExchangeOptions
	QueueOptions    RabbitMQQueueOptions
	RoutingKey      string
	ExcludeExchange bool
	AutoAck         bool
}

func NewRabbitMQEventConsumer(connection *amqp.Connection) *RabbitMQEventConsumer {
	return &RabbitMQEventConsumer{
		Connection:     connection,
		ConsumeOptions: map[string]RabbitMQConsumeEventOptions{},
		EventHandlers:  map[string]eventbus.EventHandler{},
	}
}

func (consumer *RabbitMQEventConsumer) Consume() error {
	if consumer.Connection == nil || consumer.Connection.IsClosed() || consumer.ConsumeOptions == nil {
		return nil
	}

	channel, err := consumer.Connection.Channel()
	if err != nil {
		return err
	}

	for eventName, options := range consumer.ConsumeOptions {
		exchangeOptions := options.ExchangeOptions
		queueOptions := options.QueueOptions

		if !options.ExcludeExchange && exchangeOptions.Name != "" {
			err := channel.ExchangeDeclare(
				exchangeOptions.Name,
				exchangeOptions.Type,
				exchangeOptions.Durable,
				exchangeOptions.AutoDelete,
				false,
				false,
				nil,
			)
			if err != nil {
				return err
			}
		}

		queue, err := channel.QueueDeclare(
			queueOptions.Name,
			queueOptions.Durable,
			queueOptions.AutoDelete,
			queueOptions.Exclusive,
			false,
			nil,
		)
		if err != nil {
			return err
		}

		if !options.ExcludeExchange && exchangeOptions.Name != "" {
			err = channel.QueueBind(
				queue.Name,
				options.RoutingKey,
				exchangeOptions.Name,
				false,
				nil,
			)
			if err != nil {
				return err
			}
		}

		messages, err := channel.Consume(
			queue.Name,
			"",
			options.AutoAck,
			false,
			false,
			false,
			nil,
		)
		if err != nil {
			return err
		}

		go func() {
			for msg := range messages {
				handler := consumer.EventHandlers[eventName]

				err := handler.Handle(msg.Body)

				if err != nil {
					fmt.Printf("Error consume event %s: %s\n", eventName, err.Error())
				} else if !options.AutoAck {
					channel.Ack(msg.DeliveryTag, false)
				}
			}
		}()
	}

	return nil
}

func (consumer *RabbitMQEventConsumer) ConfigureConsume(
	event interface{},
	handler eventbus.EventHandler,
	config func(*RabbitMQConsumeEventOptions),
) {
	options := RabbitMQConsumeEventOptions{
		ExchangeOptions: RabbitMQExchangeOptions{},
		QueueOptions:    RabbitMQQueueOptions{},
	}

	config(&options)

	eventName := reflect.TypeOf(event).Name()

	_, isExists := consumer.ConsumeOptions[eventName]
	if !isExists {
		consumer.ConsumeOptions[eventName] = options
	}

	_, isExists = consumer.EventHandlers[eventName]
	if !isExists {
		consumer.EventHandlers[eventName] = handler
	}
}
