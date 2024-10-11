package main

import (
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/mailer"
	"github.com/VuStrong/Vicourses/backend/services/email_service/internal/rabbitmq"
)

func main() {
	forever := make(chan int)

	cfg, err := config.LoadConfig()
	if err != nil {
		panic("Error while loading config: " + err.Error())
	}

	mailer := mailer.NewMailer(cfg)

	rabbitmqConnection, err := rabbitmq.NewRabbitMQ(cfg)
	if err != nil {
		panic("Error while connecting to RabbitMQ: " + err.Error())
	}

	consumer := rabbitmq.NewRabbitMQConsumer(rabbitmqConnection, mailer, cfg)

	go func() {
		err := consumer.Consume()

		if err != nil {
			panic("Error while start consume messages from RabbitMQ: " + err.Error())
		}
	}()

	<-forever
}
