package eventbus

type EventHandler interface {
	Handle(eventByte []byte) error
}

type EventPublisher interface {
	Publish(event interface{}) error
}
