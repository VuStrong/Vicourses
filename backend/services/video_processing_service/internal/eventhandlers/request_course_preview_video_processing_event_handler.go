package eventhandlers

import (
	"encoding/json"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/events"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/file"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/logger"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/videoprocessor"
)

type RequestCoursePreviewVideoProcessingEventHandler struct {
	Logger         logger.Logger
	EventPublisher eventbus.EventPublisher
	Cfg            *config.Config
}

func NewRequestCoursePreviewVideoProcessingEventHandler(
	l logger.Logger,
	eventPublisher eventbus.EventPublisher,
	cfg *config.Config,
) *RequestCoursePreviewVideoProcessingEventHandler {
	return &RequestCoursePreviewVideoProcessingEventHandler{
		Logger:         l,
		EventPublisher: eventPublisher,
		Cfg:            cfg,
	}
}

func (handler *RequestCoursePreviewVideoProcessingEventHandler) Handle(eventByte []byte) error {
	var event events.RequestCoursePreviewVideoProcessingEvent

	err := json.Unmarshal(eventByte, &event)
	if err != nil {
		return err
	}

	handler.Logger.Info("Handle RequestCoursePreviewVideoProcessingEvent")

	_ = videoprocessor.NewHlsEncoder(handler.Cfg.VideoEncodeHeights, 10)

	_, err = file.NewFileDownloader(handler.Cfg)
	if err != nil {
		return err
	}

	return nil
}
