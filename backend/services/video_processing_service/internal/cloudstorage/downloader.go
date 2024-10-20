package cloudstorage

import (
	"context"
	"fmt"
	"os"
	"path/filepath"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"github.com/aws/aws-sdk-go-v2/aws"
	awsConfig "github.com/aws/aws-sdk-go-v2/config"
	"github.com/aws/aws-sdk-go-v2/credentials"
	"github.com/aws/aws-sdk-go-v2/feature/s3/manager"
	"github.com/aws/aws-sdk-go-v2/service/s3"
	"github.com/google/uuid"
)

type Downloader interface {
	DownloadFile(fileId string) (string, error)
}

type s3Downloader struct {
	s3Client  *s3.Client
	appConfig *config.Config
}

func NewDownloader(cfg *config.Config) (*s3Downloader, error) {
	s3Cfg := cfg.S3

	awsCfg, err := awsConfig.LoadDefaultConfig(context.TODO(),
		awsConfig.WithCredentialsProvider(credentials.NewStaticCredentialsProvider(s3Cfg.AccessKeyId, s3Cfg.AccessKeySecret, "")),
		awsConfig.WithRegion("auto"),
	)
	if err != nil {
		return nil, err
	}

	s3Client := s3.NewFromConfig(awsCfg, func(o *s3.Options) {
		o.BaseEndpoint = aws.String(s3Cfg.Endpoint)
	})

	return &s3Downloader{s3Client: s3Client, appConfig: cfg}, nil
}

// Download the file and save to temp/ directory
func (s3Downloader *s3Downloader) DownloadFile(fileId string) (string, error) {
	var partMiBs int64 = 10
	s3Cfg := s3Downloader.appConfig.S3

	ext := filepath.Ext(fileId)
	path := fmt.Sprintf("temp/%s%s", uuid.New().String(), ext)

	err := os.MkdirAll("temp", os.ModePerm)
	if err != nil {
		return "", err
	}

	file, err := os.Create(path)
	if err != nil {
		return "", err
	}

	downloader := manager.NewDownloader(s3Downloader.s3Client, func(d *manager.Downloader) {
		d.PartSize = partMiBs * 1024 * 1024
	})

	input := &s3.GetObjectInput{
		Bucket: &s3Cfg.Bucket,
		Key:    &fileId,
	}

	_, err = downloader.Download(context.TODO(), file, input)

	file.Close()

	if err != nil {
		os.Remove(path)
		return "", err
	}

	return path, nil
}
