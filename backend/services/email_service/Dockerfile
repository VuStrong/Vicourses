FROM golang:1.22.1-alpine as builder
WORKDIR /app
COPY ./ /app
RUN go mod download

RUN CGO_ENABLED=0 GOOS=linux GOARCH=amd64 go build -a -installsuffix cgo -o main cmd/email_service/main.go

FROM alpine:latest
COPY --from=builder /app/main ./

CMD ["./main"]