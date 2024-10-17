package rabbitmq

import (
	"reflect"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/logger"
	amqp "github.com/rabbitmq/amqp091-go"
)

type RabbitMQEventConsumer struct {
	Connection     *amqp.Connection
	ConsumeOptions map[string]RabbitMQConsumeEventOptions
	EventHandlers  map[string]eventbus.EventHandler
	Logger         logger.Logger
}

type RabbitMQConsumeEventOptions struct {
	ExchangeOptions RabbitMQExchangeOptions
	QueueOptions    RabbitMQQueueOptions
	RoutingKey      string
	ExcludeExchange bool
	AutoAck         bool
}

func NewRabbitMQEventConsumer(connection *amqp.Connection, l logger.Logger) *RabbitMQEventConsumer {
	return &RabbitMQEventConsumer{
		Connection:     connection,
		Logger:         l,
		ConsumeOptions: map[string]RabbitMQConsumeEventOptions{},
		EventHandlers:  map[string]eventbus.EventHandler{},
	}
}

func (consumer *RabbitMQEventConsumer) Consume() error {
	if consumer.Connection == nil || consumer.Connection.IsClosed() {
		return nil
	}

	channel, err := consumer.Connection.Channel()
	if err != nil {
		return err
	}

	defer channel.Close()

	var forever chan struct{}

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
					consumer.Logger.Errorf("Error consume event %s: %s\n", eventName, err.Error())
				} else if !options.AutoAck {
					channel.Ack(msg.DeliveryTag, false)
				}
			}
		}()
	}

	consumer.Logger.Info("Start consuming events from RabbitMQ")

	<-forever

	return nil
}

func (consumer *RabbitMQEventConsumer) ConfigureConsume(
	event interface{},
	handler eventbus.EventHandler,
	config func(*RabbitMQConsumeEventOptions),
) {
	options := RabbitMQConsumeEventOptions{
		ExchangeOptions: RabbitMQExchangeOptions{
			Type:    Fanout,
			Durable: true,
		},
		QueueOptions: RabbitMQQueueOptions{
			Durable: true,
		},
		AutoAck: true,
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
