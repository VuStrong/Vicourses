package eventhandlers

import (
	"encoding/json"
	"os"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/cloudstorage"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/eventbus"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/events"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/logger"
	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/videoprocessor"
)

type RequestLessonVideoProcessingEventHandler struct {
	Logger         logger.Logger
	EventPublisher eventbus.EventPublisher
	Cfg            *config.Config
}

func NewRequestLessonVideoProcessingEventHandler(
	l logger.Logger,
	eventPublisher eventbus.EventPublisher,
	cfg *config.Config,
) *RequestLessonVideoProcessingEventHandler {
	return &RequestLessonVideoProcessingEventHandler{
		Logger:         l,
		EventPublisher: eventPublisher,
		Cfg:            cfg,
	}
}

func (handler *RequestLessonVideoProcessingEventHandler) Handle(eventByte []byte) error {
	var event events.RequestLessonVideoProcessingEvent

	err := json.Unmarshal(eventByte, &event)
	if err != nil {
		return err
	}

	handler.Logger.Info("Handle RequestLessonVideoProcessingEvent")

	completedEvent, err := handler.doProcess(event)
	if err != nil {
		failedEvent := &events.LessonVideoProcessingFailedEvent{LessonId: event.LessonId}

		handler.EventPublisher.Publish(failedEvent)

		return err
	}

	handler.Logger.Info("Completed video processing for lesson " + event.LessonId)

	handler.EventPublisher.Publish(completedEvent)

	return nil
}

func (handler *RequestLessonVideoProcessingEventHandler) doProcess(
	event events.RequestLessonVideoProcessingEvent,
) (*events.LessonVideoProcessingCompletedEvent, error) {
	downloader, err := cloudstorage.NewDownloader(handler.Cfg)
	if err != nil {
		return nil, err
	}

	filePath, err := downloader.DownloadFile(event.VideoUrl)
	if err != nil {
		return nil, err
	}

	duration, err := videoprocessor.GetDuration(filePath)
	if err != nil {
		os.Remove(filePath)
		return nil, err
	}

	encoder := videoprocessor.NewHlsEncoder(handler.Cfg.VideoEncodeHeights, 10)
	encodeResult, err := encoder.Encode(filePath)
	if err != nil {
		os.Remove(filePath)
		return nil, err
	}

	uploader := cloudstorage.NewUploader(handler.Cfg)
	remotePath, err := uploader.UploadDirectory(encodeResult.Path, "hls/"+encodeResult.Id)

	// Cleanup
	go func() {
		os.Remove(filePath)
		os.RemoveAll(encodeResult.Path)
	}()

	if err != nil {
		return nil, err
	}

	completedEvent := &events.LessonVideoProcessingCompletedEvent{
		LessonId:  event.LessonId,
		StreamUrl: remotePath + "/" + encodeResult.MasterFileName,
		Duration:  duration,
	}

	return completedEvent, nil
}
