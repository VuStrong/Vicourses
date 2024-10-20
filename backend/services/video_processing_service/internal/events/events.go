package events

type RequestCoursePreviewVideoProcessingEvent struct {
	CourseId string
	VideoUrl string
}

type RequestLessonVideoProcessingEvent struct {
	LessonId string
	VideoUrl string
}

type CoursePreviewVideoProcessingCompletedEvent struct {
	CourseId  string
	StreamUrl string
	Duration  int
}

type CoursePreviewVideoProcessingFailedEvent struct {
	CourseId string
}

type LessonVideoProcessingCompletedEvent struct {
	LessonId  string
	StreamUrl string
	Duration  int
}

type LessonVideoProcessingFailedEvent struct {
	LessonId string
}
