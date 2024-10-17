package rabbitmq

import (
	"encoding/json"
	"reflect"

	amqp "github.com/rabbitmq/amqp091-go"
)

type RabbitMQEventPublisher struct {
	Connection     *amqp.Connection
	PublishOptions map[string]RabbitMQPublishEventOptions
}

type RabbitMQPublishEventOptions struct {
	ExchangeOptions RabbitMQExchangeOptions
	RoutingKey      string
	ExcludeExchange bool
}

func NewRabbitMQEventPublisher(connection *amqp.Connection) *RabbitMQEventPublisher {
	return &RabbitMQEventPublisher{
		Connection:     connection,
		PublishOptions: map[string]RabbitMQPublishEventOptions{},
	}
}

func (publisher *RabbitMQEventPublisher) Publish(event interface{}) error {
	if publisher.Connection == nil || publisher.Connection.IsClosed() || publisher.PublishOptions == nil {
		return nil
	}

	body, err := json.Marshal(event)
	if err != nil {
		return err
	}

	eventName := reflect.TypeOf(event).Elem().Name()
	options, isExists := publisher.PublishOptions[eventName]

	if !isExists {
		return nil
	}

	channel, err := publisher.Connection.Channel()
	if err != nil {
		return err
	}

	defer channel.Close()

	exchangeOptions := options.ExchangeOptions

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

	err = channel.Publish(
		exchangeOptions.Name,
		options.RoutingKey,
		false,
		false,
		amqp.Publishing{
			Body: body,
		},
	)
	if err != nil {
		return err
	}

	return nil
}

func (publisher *RabbitMQEventPublisher) ConfigurePublish(
	event interface{},
	config func(*RabbitMQPublishEventOptions),
) {
	options := RabbitMQPublishEventOptions{
		ExchangeOptions: RabbitMQExchangeOptions{
			Type:    Fanout,
			Durable: true,
		},
	}

	config(&options)

	eventName := reflect.TypeOf(event).Name()

	_, isExists := publisher.PublishOptions[eventName]
	if !isExists {
		publisher.PublishOptions[eventName] = options
	}
}
