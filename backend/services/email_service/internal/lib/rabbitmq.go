package lib

import (
	"encoding/json"
	"fmt"

	amqp "github.com/rabbitmq/amqp091-go"
)

type RabbitMQ struct {
	Connection *amqp.Connection
	Channel    *amqp.Channel
}

type SendLinkBody struct {
	User User
	Link string
}

type EmailQueueBody struct {
	EmailType string
}

func ConnectRabbitmq() *RabbitMQ {
	var err error
	rmq := &RabbitMQ{}

	rabbitConf := Conf.RabbitMQ
	rmq.Connection, err = amqp.Dial(fmt.Sprintf("amqp://%s:%s@%s/", rabbitConf.User, rabbitConf.Password, rabbitConf.Host))
	if err != nil {
		panic(err)
	}

	rmq.Channel, err = rmq.Connection.Channel()
	if err != nil {
		panic(err)
	}

	rmq.Channel.QueueDeclare("send_email", true, false, false, true, nil)

	return rmq
}

func _consumeEmail(msg []byte) {
	var body EmailQueueBody
	err := json.Unmarshal(msg, &body)

	if err != nil {
		panic(err)
	}

	switch body.EmailType {
	case "confirm_email":
		var sendEmailBody SendLinkBody
		json.Unmarshal(msg, &sendEmailBody)

		go SendConfirmEmailLink(sendEmailBody.User, sendEmailBody.Link)
	case "reset_password":
		var sendEmailBody SendLinkBody
		json.Unmarshal(msg, &sendEmailBody)

		go SendResetPasswordLink(sendEmailBody.User, sendEmailBody.Link)
	}
}

func (rmq *RabbitMQ) StartConsume() {
	defer rmq.Channel.Close()
	defer rmq.Connection.Close()

	msgs, err := rmq.Channel.Consume(
		"send_email",
		"",
		true,
		false,
		false,
		false,
		nil,
	)

	if err != nil {
		panic(err)
	}

	fmt.Println("Start consume...")

	for d := range msgs {
		_consumeEmail(d.Body)
	}
}
