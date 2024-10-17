package main

import (
	"log"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus/rabbitmq"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventhandlers"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/events"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/logger"

	amqp "github.com/rabbitmq/amqp091-go"
)

func main() {
	log.Println("Starting video processing service")

	forever := make(chan int)

	cfg, err := config.LoadConfig()
	if err != nil {
		panic("Error loading config: " + err.Error())
	}

	appLogger := logger.NewAppLogger(cfg)
	appLogger.InitLogger()

	rabbitmqConnection, err := rabbitmq.NewRabbitMQ(cfg)
	if err != nil {
		panic("Error connecting to RabbitMQ: " + err.Error())
	}

	publisher := configureRabbitMQPublisher(rabbitmqConnection)

	configureRabbitMQConsumer(rabbitmqConnection, appLogger, publisher, cfg)

	<-forever
}

func configureRabbitMQPublisher(rabbitmqConnection *amqp.Connection) *rabbitmq.RabbitMQEventPublisher {
	publisher := rabbitmq.NewRabbitMQEventPublisher(rabbitmqConnection)

	publisher.ConfigurePublish(events.CoursePreviewVideoProcessingCompletedEvent{}, func(o *rabbitmq.RabbitMQPublishEventOptions) {
		o.ExchangeOptions.Name = "course-preview-video-processing.completed"
	})

	return publisher
}

func configureRabbitMQConsumer(
	rabbitmqConnection *amqp.Connection,
	l logger.Logger,
	publisher eventbus.EventPublisher,
	cfg *config.Config,
) {
	consumer := rabbitmq.NewRabbitMQEventConsumer(rabbitmqConnection, l)

	consumer.ConfigureConsume(
		events.RequestCoursePreviewVideoProcessingEvent{},
		eventhandlers.NewRequestCoursePreviewVideoProcessingEventHandler(l, publisher, cfg),
		func(o *rabbitmq.RabbitMQConsumeEventOptions) {
			o.ExcludeExchange = true

			o.QueueOptions.Name = "process_course_preview_video"
		},
	)
	consumer.ConfigureConsume(
		events.RequestLessionVideoProcessingEvent{},
		eventhandlers.NewRequestLessionVideoProcessingEventHandler(l, publisher, cfg),
		func(o *rabbitmq.RabbitMQConsumeEventOptions) {
			o.ExcludeExchange = true

			o.QueueOptions.Name = "process_lession_video"
		},
	)

	go func() {
		err := consumer.Consume()

		if err != nil {
			panic("Error start consume event from RabbitMQ: " + err.Error())
		}
	}()
}
