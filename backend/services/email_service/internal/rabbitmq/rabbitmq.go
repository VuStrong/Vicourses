package rabbitmq

import (
	"encoding/json"
	"fmt"

	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/emails"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/mailer"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/models"

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

	fmt.Println("Start consume...")

	for msg := range messages {
		err := onMessageReceived(msg, consumer.Mailer, consumer.Config)

		if err != nil {
			fmt.Println("Error when sending email: " + err.Error())
		}
	}

	<-forever

	return nil
}

func onMessageReceived(d amqp.Delivery, mailer *mailer.Mailer, cfg *config.Config) error {
	var msg emails.BaseEmail
	var err error
	err = json.Unmarshal(d.Body, &msg)

	if err != nil {
		return err
	}

	var htmlGenerator emails.HtmlGenerator
	var subject string

	switch msg.EmailType {
	case "confirm_email":
		var confirmEmail emails.ConfirmEmail
		err = json.Unmarshal(d.Body, &confirmEmail)
		confirmEmail.AppName = cfg.AppName
		confirmEmail.WebUrl = cfg.WebUrl
		confirmEmail.AppLogoUrl = cfg.AppLogoUrl
		htmlGenerator = &confirmEmail
		subject = "Confirm Email"
	case "reset_password":
		var resetPassEmail emails.ResetPasswordEmail
		err = json.Unmarshal(d.Body, &resetPassEmail)
		resetPassEmail.AppName = cfg.AppName
		resetPassEmail.WebUrl = cfg.WebUrl
		resetPassEmail.AppLogoUrl = cfg.AppLogoUrl
		htmlGenerator = &resetPassEmail
		subject = "Reset Password"
	default:
		return fmt.Errorf("EmailType %s is invalid", msg.EmailType)
	}

	if err != nil {
		return err
	}

	html, err := htmlGenerator.GenerateHTML()
	if err != nil {
		return err
	}

	emailModel := models.EmailModel{
		From:        cfg.Smtp.User,
		To:          msg.To,
		ContentType: "text/html",
		Body:        html,
		Subject:     subject,
	}

	err = mailer.SendEmail(&emailModel)

	// Add emailModel to database (optinal)

	return err
}
