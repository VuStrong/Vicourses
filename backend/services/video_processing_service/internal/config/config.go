package config

import (
	"os"
	"strconv"
	"strings"

	"github.com/joho/godotenv"
)

type Config struct {
	RabbitMQUri        string
	S3                 S3
	VideoEncodeHeights []int
}

type S3 struct {
	Endpoint        string
	AccountId       string
	AccessKeyId     string
	AccessKeySecret string
	Bucket          string
	Domain          string
}

func LoadConfig() (*Config, error) {
	config := Config{}

	err := godotenv.Load()
	if err != nil {
		return nil, err
	}

	config.RabbitMQUri = os.Getenv("RABBITMQ_URI")
	config.S3 = S3{
		Endpoint:        os.Getenv("S3_ENDPOINT"),
		AccountId:       os.Getenv("S3_ACCOUNT_ID"),
		AccessKeyId:     os.Getenv("S3_ACCESS_KEY_ID"),
		AccessKeySecret: os.Getenv("S3_ACCESS_KEY_SECRET"),
		Bucket:          os.Getenv("S3_BUCKET_NAME"),
		Domain:          os.Getenv("S3_DOMAIN"),
	}

	config.VideoEncodeHeights = []int{}

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
