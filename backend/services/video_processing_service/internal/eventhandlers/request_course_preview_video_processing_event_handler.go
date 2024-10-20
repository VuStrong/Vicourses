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

	completedEvent, err := handler.doProcess(event)
	if err != nil {
		failedEvent := &events.CoursePreviewVideoProcessingFailedEvent{CourseId: event.CourseId}

		handler.EventPublisher.Publish(failedEvent)

		return err
	}

	handler.Logger.Info("Completed preview video processing for course " + event.CourseId)

	handler.EventPublisher.Publish(completedEvent)

	return nil
}

func (handler *RequestCoursePreviewVideoProcessingEventHandler) doProcess(
	event events.RequestCoursePreviewVideoProcessingEvent,
) (*events.CoursePreviewVideoProcessingCompletedEvent, error) {
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

	completedEvent := &events.CoursePreviewVideoProcessingCompletedEvent{
		CourseId:  event.CourseId,
		StreamUrl: remotePath + "/" + encodeResult.MasterFileName,
		Duration:  duration,
	}

	return completedEvent, nil
}
