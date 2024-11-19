package rabbitmq

import (
	"encoding/json"
	"log"

	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/emailgenerator"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/mailer"

	amqp "github.com/rabbitmq/amqp091-go"
)

type RabbitMQConsumer struct {
	Connection *amqp.Connection
	Mailer     *mailer.Mailer
	Config     *config.Config
}

func NewRabbitMQ(c *config.Config) (*amqp.Connection, error) {
	connection, err := amqp.Dial(c.RabbitMQUri)
	if err != nil {
		return nil, err
	}

	return connection, nil
}

func NewRabbitMQConsumer(connection *amqp.Connection, mailer *mailer.Mailer, cfg *config.Config) *RabbitMQConsumer {
	return &RabbitMQConsumer{
		Connection: connection,
		Mailer:     mailer,
		Config:     cfg,
	}
}

func (consumer *RabbitMQConsumer) Consume() error {
	channel, err := consumer.Connection.Channel()
	if err != nil {
		return err
	}

	queue, err := channel.QueueDeclare("send_email", true, false, false, true, nil)
	if err != nil {
		return err
	}

	defer channel.Close()

	var forever chan struct{}

	messages, err := channel.Consume(
		queue.Name,
		"",
		true,
		false,
		false,
		false,
		nil,
	)
	if err != nil {
		return err
	}

	log.Println("Start consume...")

	for msg := range messages {
		err := onMessageReceived(msg, consumer.Mailer, consumer.Config)

		if err != nil {
			log.Println("Error when sending email: " + err.Error())
		}
	}

	<-forever

	return nil
}

func onMessageReceived(d amqp.Delivery, mailer *mailer.Mailer, cfg *config.Config) error {
	var generator emailgenerator.EmailGenerator
	err := json.Unmarshal(d.Body, &generator)

	if err != nil {
		return err
	}

	generator.From = cfg.Smtp.User
	generator.AppName = cfg.AppName
	generator.AppLogoUrl = cfg.AppLogoUrl
	generator.WebUrl = cfg.WebUrl

	emailModel, err := generator.Generate()
	if err != nil {
		return err
	}

	err = mailer.SendEmail(emailModel)
	if err != nil {
		return err
	}

	log.Println("Email has been sent to the user " + emailModel.To)

	// Todo: Add emailModel to database

	return nil
}
