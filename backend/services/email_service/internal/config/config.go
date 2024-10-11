package config

import (
	"os"
	"strconv"

	"github.com/joho/godotenv"
)

type Config struct {
	Smtp struct {
		Host     string
		Port     int
		User     string
		Password string
	}
	RabbitMQUri string
	AppName     string
	AppLogoUrl  string
	WebUrl      string
}

func LoadConfig() (*Config, error) {
	config := Config{}

	err := godotenv.Load()
	if err != nil {
		return nil, err
	}

	port, err := strconv.Atoi(os.Getenv("SMTP_PORT"))
	if err != nil {
		return nil, err
	}

	config.Smtp = struct {
		Host     string
		Port     int
		User     string
		Password string
	}{
		Host:     os.Getenv("SMTP_HOST"),
		Port:     port,
		User:     os.Getenv("SMTP_USER"),
		Password: os.Getenv("SMTP_PASS"),
	}
	config.RabbitMQUri = os.Getenv("RABBITMQ_URI")
	config.AppName = os.Getenv("APP_NAME")
	config.AppLogoUrl = os.Getenv("APP_LOGO_URL")
	config.WebUrl = os.Getenv("WEB_URL")

	return &config, nil
}
