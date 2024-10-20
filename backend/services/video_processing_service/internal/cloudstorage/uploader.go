package cloudstorage

import (
	"fmt"
	"os/exec"

	"github.com/VuStrong/Vicourses/backend/services/video_processing_service/internal/config"
)

type Uploader interface {
	UploadDirectory(path string) error
}

type s3Uploader struct {
	cfg *config.Config
}

func NewUploader(cfg *config.Config) *s3Uploader {
	return &s3Uploader{
		cfg: cfg,
	}
}

// upload all files in localPath to destPath on cloud storage, return full path of cloud directory
func (uploader *s3Uploader) UploadDirectory(localPath string, destPath string) (string, error) {
	remotePath := fmt.Sprintf(
		"%s:%s/%s",
		uploader.cfg.RcloneRemote,
		uploader.cfg.S3.Bucket,
		destPath,
	)

	cmdArgs := []string{"copy", localPath, remotePath}

	cmd := exec.Command("rclone", cmdArgs...)

	err := cmd.Run()
	if err != nil {
		return "", err
	}

	return fmt.Sprintf(
		"%s/%s",
		uploader.cfg.S3.Domain,
		destPath,
	), nil
}
