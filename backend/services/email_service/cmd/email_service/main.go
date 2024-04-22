package main

import "github.com/VuStrong/Vicourses/backend/services/email_service/internal/lib"

func main() {
	forever := make(chan int)

	lib.LoadConfig()

	rabbitmq := lib.ConnectRabbitmq()

	go rabbitmq.StartConsume()

	<-forever
}
