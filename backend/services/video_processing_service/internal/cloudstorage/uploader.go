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

func (uploader *s3Uploader) UploadDirectory(localPath string, destPath string) error {
	remotePath := fmt.Sprintf(
		"%s:%s/%s",
		uploader.cfg.RcloneRemote,
		uploader.cfg.AWS.S3Bucket,
		destPath,
	)

	cmdArgs := []string{"copy", localPath, remotePath}

	cmd := exec.Command("rclone", cmdArgs...)

	err := cmd.Run()
	if err != nil {
		return err
	}

	return nil
}
