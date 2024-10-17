package events

type RequestCoursePreviewVideoProcessingEvent struct {
	CourseId string
	VideoUrl string
}

type RequestLessionVideoProcessingEvent struct {
	LessionId string
	VideoUrl  string
}

type CoursePreviewVideoProcessingCompletedEvent struct {
	CourseId  string
	StreamUrl string
	Duration  int
}

type CoursePreviewVideoProcessingFailedEvent struct {
	CourseId string
}

type LessionVideoProcessingCompletedEvent struct {
	LessionId string
	StreamUrl string
	Duration  int
}

type LessionVideoProcessingFailedEvent struct {
	LessionId string
}
