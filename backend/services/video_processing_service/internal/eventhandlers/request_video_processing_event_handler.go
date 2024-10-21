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

type RequestVideoProcessingEventHandler struct {
	Logger         logger.Logger
	EventPublisher eventbus.EventPublisher
	Cfg            *config.Config
}

func NewRequestVideoProcessingEventHandler(
	l logger.Logger,
	eventPublisher eventbus.EventPublisher,
	cfg *config.Config,
) *RequestVideoProcessingEventHandler {
	return &RequestVideoProcessingEventHandler{
		Logger:         l,
		EventPublisher: eventPublisher,
		Cfg:            cfg,
	}
}

func (handler *RequestVideoProcessingEventHandler) Handle(eventByte []byte) error {
	var event events.RequestVideoProcessingEvent

	err := json.Unmarshal(eventByte, &event)
	if err != nil {
		return err
	}

	handler.Logger.Info("Handle RequestVideoProcessingEvent")

	completedEvent, err := handler.doProcess(event)
	if err != nil {
		failedEvent := &events.VideoProcessingFailedEvent{
			Entity:   event.Entity,
			EntityId: event.EntityId,
		}

		handler.EventPublisher.Publish(failedEvent)

		return err
	}

	handler.Logger.Infof("Completed video processing for %s: %s", event.Entity, event.EntityId)

	handler.EventPublisher.Publish(completedEvent)

	return nil
}

func (handler *RequestVideoProcessingEventHandler) doProcess(
	event events.RequestVideoProcessingEvent,
) (*events.VideoProcessingCompletedEvent, error) {
	downloader, err := cloudstorage.NewDownloader(handler.Cfg)
	if err != nil {
		return nil, err
	}

	filePath, err := downloader.DownloadFile(event.FileId)
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

	completedEvent := &events.VideoProcessingCompletedEvent{
		StreamFileUrl: remotePath + "/" + encodeResult.MasterFileName,
		Duration:      duration,
		Entity:        event.Entity,
		EntityId:      event.EntityId,
	}

	return completedEvent, nil
}
