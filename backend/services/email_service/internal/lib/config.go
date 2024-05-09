package lib

import (
	"fmt"
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
	RabbitMQ_URL string
}

var Conf *Config

func LoadConfig() {
	err := godotenv.Load()
	if err != nil {
		fmt.Println("Error loading .env file")
	}

	Conf = new(Config)
	port, err := strconv.Atoi(os.Getenv("SMTP_PORT"))
	if err != nil {
		panic(err)
	}

	Conf.Smtp = struct {
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
	Conf.RabbitMQ_URL = os.Getenv("RABBITMQ_URL")
}
