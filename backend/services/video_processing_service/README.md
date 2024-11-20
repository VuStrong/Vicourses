
# Vicourses Video Processing Service

The Video Processing Service is responsible for processing the video files. This service use **FFmpeg** to convert video into HLS streaming segments with different resolutions.

This Service consume messages from **request_video** queue, download the video, processing video and then upload the processed video to S3. If the processing fails, publish a message to **video.processing.failed** exchange. If the processing success, publish a message to **video.processing.completed** exchange.

## Technologies
- Golang
- FFmpeg
- RabbitMQ

## Setup enviroment variables
For this service to run. Golang, FFmpeg and Rclone must be installed on your machine. If you don't want to install them manually, you can use docker to run the `Dockerfile`.

1. Rename `.env.example` file to `.env`
2. Provide your S3 credentials for keys with prefix `S3_`
3. You can change value of the key `VIDEO_ENCODE_HEIGHTS` with resolutions you want to encode videos. Default is `360,720`.
4. For keys with `RCLONE_` prefix, change `{remote}` to `MYREMOTE` and provide your S3 credentials

## Run this service
```shell
  go get ./...
  go run cmd/video_processing_service/main.go
```