package videoprocessor

import (
	"bytes"
	"math"
	"os/exec"
	"strconv"
	"strings"
)

func GetResolution(videoPath string) (int, int, error) {
	cmdArgs := []string{
		"-v", "error", "-select_streams", "v:0", "-show_entries",
		"stream=width,height", "-of", "csv=s=x:p=0", videoPath,
	}

	cmd := exec.Command("ffprobe", cmdArgs...)

	var out bytes.Buffer
	cmd.Stdout = &out
	err := cmd.Run()
	if err != nil {
		return 0, 0, err
	}

	outStr := strings.TrimSpace(out.String())
	resolution := strings.Split(outStr, "x")

	width, err := strconv.Atoi(resolution[0])
	if err != nil {
		return 0, 0, err
	}

	height, err := strconv.Atoi(resolution[1])
	if err != nil {
		return 0, 0, err
	}

	return width, height, nil
}

func GetBitrate(videoPath string) (int, error) {
	cmdArgs := []string{
		"-v", "error", "-select_streams", "v:0", "-show_entries",
		"stream=bit_rate", "-of", "default=nw=1:nk=1", videoPath,
	}

	cmd := exec.Command("ffprobe", cmdArgs...)

	var out bytes.Buffer
	cmd.Stdout = &out
	err := cmd.Run()
	if err != nil {
		return 0, err
	}

	outStr := strings.TrimSpace(out.String())
	bitrate, err := strconv.Atoi(outStr)
	if err != nil {
		return 0, err
	}

	return bitrate, nil
}

// Get video duration in seconds
func GetDuration(videoPath string) (int, error) {
	cmdArgs := []string{
		"-i", videoPath, "-show_entries", "format=duration", "-v", "error", "-of", "csv=p=0",
	}

	cmd := exec.Command("ffprobe", cmdArgs...)

	var out bytes.Buffer
	cmd.Stdout = &out
	err := cmd.Run()
	if err != nil {
		return 0, err
	}

	outStr := strings.TrimSpace(out.String())
	duration, err := strconv.ParseFloat(outStr, 64)
	if err != nil {
		return 0, err
	}

	return int(math.Round(duration)), nil
}

func CheckVideoHasAudio(videoPath string) (bool, error) {
	cmdArgs := []string{
		"-v", "error", "-select_streams", "a:0", "-show_entries",
		"stream=codec_type", "-of", "default=nw=1:nk=1", videoPath,
	}

	cmd := exec.Command("ffprobe", cmdArgs...)

	var out bytes.Buffer
	cmd.Stdout = &out
	err := cmd.Run()
	if err != nil {
		return false, err
	}

	outStr := strings.TrimSpace(out.String())

	return outStr == "audio", nil
}
