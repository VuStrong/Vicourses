package lib

import (
	"fmt"
	"os"

	"gopkg.in/yaml.v3"
)

type Config struct {
	Smtp struct {
		Host     string
		Port     int
		User     string
		Password string
	}
	RabbitMQ struct {
		Host     string
		User     string
		Password string
	}
}

var Conf *Config

func LoadConfig() {
	env := os.Getenv("ENV")

	var configPath string
	if env != "" {
		configPath = fmt.Sprintf("config/config.%s.yml", env)
	} else {
		configPath = "config/config.dev.yml"
	}

	fileData, err := os.ReadFile(configPath)
	if err != nil {
		panic(err)
	}

	Conf = new(Config)

	err = yaml.Unmarshal(fileData, Conf)

	if err != nil {
		panic(err)
	}
}
