module github.com/VuStrong/Vicourses/backend/services/video_processing_service

go 1.22.1

require github.com/joho/godotenv v1.5.1

require (
	github.com/aws/aws-sdk-go-v2 v1.32.2
	github.com/aws/aws-sdk-go-v2/config v1.27.43
	github.com/aws/aws-sdk-go-v2/credentials v1.17.41
	github.com/aws/aws-sdk-go-v2/feature/s3/manager v1.17.32
	github.com/aws/aws-sdk-go-v2/service/s3 v1.65.3
	github.com/google/uuid v1.6.0
	github.com/rabbitmq/amqp091-go v1.10.0
)

require (
	github.com/aws/aws-sdk-go-v2/aws/protocol/eventstream v1.6.6 // indirect
	github.com/aws/aws-sdk-go-v2/feature/ec2/imds v1.16.17 // indirect
	github.com/aws/aws-sdk-go-v2/internal/configsources v1.3.21 // indirect
	github.com/aws/aws-sdk-go-v2/internal/endpoints/v2 v2.6.21 // indirect
	github.com/aws/aws-sdk-go-v2/internal/ini v1.8.1 // indirect
	github.com/aws/aws-sdk-go-v2/internal/v4a v1.3.21 // indirect
	github.com/aws/aws-sdk-go-v2/service/internal/accept-encoding v1.12.0 // indirect
	github.com/aws/aws-sdk-go-v2/service/internal/checksum v1.4.2 // indirect
	github.com/aws/aws-sdk-go-v2/service/internal/presigned-url v1.12.2 // indirect
	github.com/aws/aws-sdk-go-v2/service/internal/s3shared v1.18.2 // indirect
	github.com/aws/aws-sdk-go-v2/service/sso v1.24.2 // indirect
	github.com/aws/aws-sdk-go-v2/service/ssooidc v1.28.2 // indirect
	github.com/aws/aws-sdk-go-v2/service/sts v1.32.2 // indirect
	github.com/aws/smithy-go v1.22.0 // indirect
	go.uber.org/multierr v1.11.0 // indirect
	go.uber.org/zap v1.27.0 // indirect
	gopkg.in/natefinch/lumberjack.v2 v2.2.1 // indirect
)
