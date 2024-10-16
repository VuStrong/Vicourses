package eventhandlers

import (
	"encoding/json"
	"fmt"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/events"
)

type RequestCoursePreviewVideoProcessingEventHandler struct {
}

func (handler *RequestCoursePreviewVideoProcessingEventHandler) Handle(event []byte) error {
	var e events.RequestCoursePreviewVideoProcessingEvent
	err := json.Unmarshal(event, &e)
	if err != nil {
		return err
	}

	fmt.Println("Handle RequestCoursePreviewVideoProcessingEventHandler: ")
	fmt.Println("CourseId: " + e.CourseId)
	fmt.Println("VideoUrl: " + e.VideoUrl)
	fmt.Println("=================")

	return nil
}
