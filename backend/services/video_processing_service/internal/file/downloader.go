package file

import (
	"context"
	"errors"
	"fmt"
	"os"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
	"github.com/aws/aws-sdk-go-v2/aws"
	awsConfig "github.com/aws/aws-sdk-go-v2/config"
	"github.com/aws/aws-sdk-go-v2/credentials"
	"github.com/aws/aws-sdk-go-v2/feature/s3/manager"
	"github.com/aws/aws-sdk-go-v2/service/s3"
)

type S3FileDownloader struct {
	S3Client  *s3.Client
	AppConfig *config.Config
}

func NewS3FileDownloader(cfg *config.Config) (*S3FileDownloader, error) {
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

	return &S3FileDownloader{S3Client: s3Client, AppConfig: cfg}, nil
}

func (s3FileDownloader *S3FileDownloader) DownloadFile(fileId string) (string, error) {
	var partMiBs int64 = 10
	folder := "temp"
	path := fmt.Sprintf("%s/%s", folder, fileId)
	s3Cfg := s3FileDownloader.AppConfig.S3

	if _, err := os.Stat(folder); errors.Is(err, os.ErrNotExist) {
		err := os.Mkdir(folder, os.ModePerm)
		if err != nil {
			return "", err
		}
	}

	file, err := os.Create(path)
	if err != nil {
		return "", err
	}

	defer file.Close()

	downloader := manager.NewDownloader(s3FileDownloader.S3Client, func(d *manager.Downloader) {
		d.PartSize = partMiBs * 1024 * 1024
	})

	input := &s3.GetObjectInput{
		Bucket: &s3Cfg.Bucket,
		Key:    &fileId,
	}

	_, err = downloader.Download(context.TODO(), file, input)
	if err != nil {
		file.Close()
		os.Remove(path)
		return "", err
	}

	return path, nil
}
