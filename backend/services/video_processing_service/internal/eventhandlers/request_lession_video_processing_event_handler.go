package eventhandlers

import (
	"encoding/json"
	"fmt"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/events"
)

type RequestLessionVideoProcessingEventHandler struct {
}

func (handler *RequestLessionVideoProcessingEventHandler) Handle(event []byte) error {
	var e events.RequestLessionVideoProcessingEvent
	err := json.Unmarshal(event, &e)
	if err != nil {
		return err
	}

	fmt.Println("Handle RequestLessionVideoProcessingEventHandler: ")
	fmt.Println("LessionId: " + e.LessionId)
	fmt.Println("VideoUrl: " + e.VideoUrl)
	fmt.Println("=================")

	return nil
}
