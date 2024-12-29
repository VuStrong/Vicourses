package config

import (
	"os"
	"strconv"
	"strings"

	"github.com/joho/godotenv"
)

type Config struct {
	RabbitMQUri        string
	AWS                AWS
	Logger             Logger
	VideoEncodeHeights []int
	RcloneRemote       string
}

type AWS struct {
	AccessKey string
	SecretKey string
	Region    string
	S3Bucket  string
}

type Logger struct {
	Level string
}

func LoadConfig() (*Config, error) {
	config := Config{}

	godotenv.Load()

	config.RabbitMQUri = os.Getenv("RABBITMQ_URI")
	config.AWS = AWS{
		AccessKey: os.Getenv("AWS_ACCESS_KEY"),
		SecretKey: os.Getenv("AWS_SECRET_KEY"),
		Region:    os.Getenv("AWS_REGION"),
		S3Bucket:  os.Getenv("AWS_S3_BUCKET_NAME"),
	}

	config.Logger = Logger{
		Level: os.Getenv("LOGGER_LEVEL"),
	}

	config.VideoEncodeHeights = []int{}

	config.RcloneRemote = os.Getenv("RCLONE_REMOTE")

	heightsStr := os.Getenv("VIDEO_ENCODE_HEIGHTS")
	heightArr := strings.Split(heightsStr, ",")
	for _, value := range heightArr {
		height, err := strconv.Atoi(value)
		if err != nil {
			return nil, err
		}

		config.VideoEncodeHeights = append(config.VideoEncodeHeights, height)
	}

	return &config, nil
}
