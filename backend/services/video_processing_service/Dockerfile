FROM golang:1.22.1-alpine as builder
WORKDIR /app
COPY ./ /app
RUN go mod download

RUN CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -a -installsuffix cgo -o main cmd/video_processing_service/main.go

FROM alpine:latest
COPY --from=builder /app/main ./

RUN apk update
RUN apk add --no-cache ffmpeg
RUN apk add --no-cache curl
RUN curl -O https://downloads.rclone.org/rclone-current-linux-amd64.zip && \
    unzip rclone-current-linux-amd64.zip && \
    cd rclone-*-linux-amd64 && \
    cp rclone /usr/bin/ && \
    chmod +x /usr/bin/rclone && \
    rm -rf /rclone* && \
    apk del curl

CMD ["./main"]