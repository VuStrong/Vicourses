package events

type RequestVideoProcessingEvent struct {
	FileId   string
	Url      string
	Entity   string
	EntityId string
}

type VideoProcessingCompletedEvent struct {
	StreamFileUrl string
	Duration      int
	Entity        string
	EntityId      string
}

type VideoProcessingFailedEvent struct {
	Entity   string
	EntityId string
}
