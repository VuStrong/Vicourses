package eventbus

type EventHandler interface {
	Handle(event []byte) error
}

type EventPublisher interface {
	Publish(event interface{}) error
}
