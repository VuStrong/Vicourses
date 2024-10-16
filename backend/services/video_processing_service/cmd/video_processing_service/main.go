package main

import (
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus/rabbitmq"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventhandlers"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/events"
)

func main() {
	forever := make(chan int)

	cfg, err := config.LoadConfig()
	if err != nil {
		panic("Error while loading config: " + err.Error())
	}

	addRabbitMQ(cfg)

	<-forever
}

func addRabbitMQ(cfg *config.Config) {
	rabbitmqConnection, err := rabbitmq.NewRabbitMQ(cfg)
	if err != nil {
		panic("Error connecting to RabbitMQ: " + err.Error())
	}

	consumer := rabbitmq.NewRabbitMQEventConsumer(rabbitmqConnection)

	consumer.ConfigureConsume(
		events.RequestCoursePreviewVideoProcessingEvent{},
		&eventhandlers.RequestCoursePreviewVideoProcessingEventHandler{},
		func(o *rabbitmq.RabbitMQConsumeEventOptions) {
			o.ExcludeExchange = true
			o.AutoAck = true

			o.QueueOptions.Name = "process_course_preview_video"
			o.QueueOptions.Durable = true
		},
	)
	consumer.ConfigureConsume(
		events.RequestLessionVideoProcessingEvent{},
		&eventhandlers.RequestLessionVideoProcessingEventHandler{},
		func(o *rabbitmq.RabbitMQConsumeEventOptions) {
			o.ExcludeExchange = true
			o.AutoAck = true

			o.QueueOptions.Name = "process_lession_video"
			o.QueueOptions.Durable = true
		},
	)

	err = consumer.Consume()

	if err != nil {
		panic("Error start consume event from RabbitMQ: " + err.Error())
	}
}
