package videoprocessor

import (
	"errors"
	"fmt"
	"math"
	"os"
	"os/exec"
	"strconv"
	"strings"

	"github.com/google/uuid"
)

const (
	MaxBitrate360P  = 365000
	MaxBitrate480P  = 500000
	MaxBitrate720P  = 2800000
	MaxBitrate1080P = 5000000
)

// The HlsEncoder structure that calls Encode() to convert video into HLS streaming segments
type HlsEncoder struct {
	// Resolutions for encoding.
	// The encoder will select resolutions lower than or equal to the input video resolution for encoding
	Heights []int

	// Duration of each HLS segment in seconds
	SegmentTime int
}

func NewHlsEncoder(heights []int, segmentTime int) *HlsEncoder {
	if len(heights) == 0 {
		heights = append(heights, 360)
	}
	if segmentTime <= 0 {
		segmentTime = 10
	}

	return &HlsEncoder{
		Heights:     heights,
		SegmentTime: segmentTime,
	}
}

func (encoder *HlsEncoder) Encode(videoPath string) (string, error) {
	if _, err := os.Stat("temp/hls"); errors.Is(err, os.ErrNotExist) {
		err := os.Mkdir("temp/hls", os.ModePerm)
		if err != nil {
			return "", err
		}
	}

	width, height, err := GetResolution(videoPath)
	if err != nil {
		return "", err
	}
	bitrate, err := GetBitrate(videoPath)
	if err != nil {
		return "", err
	}
	isHasAudio, err := CheckVideoHasAudio(videoPath)
	if err != nil {
		return "", err
	}

	dir := "temp/hls/" + uuid.New().String()

	cmdArgs := encoder.buildArgs(videoPath, width, height, isHasAudio, bitrate, dir)

	cmd := exec.Command("ffmpeg", cmdArgs...)

	err = cmd.Run()
	if err != nil {
		return "", err
	}

	return dir, nil
}

func calculateBitrate(bitrate int, height int) int {
	max := 0

	if height <= 360 {
		max = MaxBitrate360P
	} else if height <= 480 {
		max = MaxBitrate480P
	} else if height <= 720 {
		max = MaxBitrate720P
	} else if height <= 1080 {
		max = MaxBitrate1080P
	} else {
		max = bitrate
	}

	if bitrate > max {
		return max
	} else {
		return bitrate
	}
}

func (encoder *HlsEncoder) buildArgs(videoPath string, width int, height int, isHasAudio bool,
	bitrate int, directory string) []string {

	segmentName := directory + "/v%v/segment_%d.ts"
	plName := directory + "/v%v/prog_index.m3u8"

	cmdArgs := []string{
		"-y", "-i", videoPath,
		"-preset", "slow", "-g", "48", "-sc_threshold", "0",
	}

	for _, value := range encoder.Heights {
		if height < value {
			continue
		}

		if isHasAudio {
			cmdArgs = append(cmdArgs, "-map", "0:0", "-map", "0:1")
		} else {
			cmdArgs = append(cmdArgs, "-map", "0:0")
		}
	}
	for index, heightToEncode := range encoder.Heights {
		if height < heightToEncode {
			continue
		}

		widthToEncode := int(math.Ceil(float64(heightToEncode) * float64(width) / float64(height)))
		if widthToEncode%2 != 0 {
			widthToEncode += 1
		}

		cmdArgs = append(
			cmdArgs,
			fmt.Sprintf("-s:v:%d", index),
			fmt.Sprintf("%dx%d", widthToEncode, heightToEncode),
			fmt.Sprintf("-c:v:%d", index),
			"libx264",
			fmt.Sprintf("-b:v:%d", index),
			strconv.Itoa(calculateBitrate(bitrate, heightToEncode)),
		)
	}

	cmdArgs = append(cmdArgs, "-c:a", "copy", "-var_stream_map")

	str := []string{}
	for index, value := range encoder.Heights {
		if height < value {
			continue
		}

		if isHasAudio {
			str = append(str, fmt.Sprintf("v:%d,a:%d,name:%d", index, index, value))
		} else {
			str = append(str, fmt.Sprintf("v:%d,name:%d", index, value))
		}
	}

	cmdArgs = append(cmdArgs, strings.Join(str, " "))
	cmdArgs = append(cmdArgs, "-master_pl_name", "master.m3u8", "-f", "hls",
		"-hls_time", strconv.Itoa(encoder.SegmentTime),
		"-hls_list_size", "0",
		"-hls_segment_filename", segmentName, plName)

	return cmdArgs
}
