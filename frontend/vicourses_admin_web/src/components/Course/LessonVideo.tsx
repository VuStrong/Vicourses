import { useState } from "react";
import { Lesson } from "../../types/course";
import { VideoFile } from "../../types/common";
import HlsVideoPlayer from "../HlsVideoPlayer";
import axiosInstance from "../../libs/axios";
import { formatLength } from "../../libs/utils";
import Button from "../Button";

export default function LessonVideo({ lesson }: { lesson: Lesson }) {
    const [video, setVideo] = useState<VideoFile | null>(lesson.video);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    const loadVideo = async () => {
        setIsLoading(true);

        const response = await axiosInstance.get<Lesson>(
            `/api/cs/v1/lessons/${lesson.id}`,
        );

        setVideo(response.data.video);
        setIsLoading(false);
    };

    return (
        <div>
            {video ? (
                <div className="mb-5">
                    <div>
                        <span className="font-semibold">Status:</span>{" "}
                        {video.status}
                    </div>
                    <div className="mb-5">
                        <span className="font-semibold">Duration:</span>{" "}
                        {formatLength(video.duration)}
                    </div>

                    {!video.token && video.status === "Processed" && (
                        <Button
                            loading={isLoading}
                            variant="text"
                            onClick={loadVideo}
                            className="border"
                        >
                            Load video
                        </Button>
                    )}
                </div>
            ) : (
                <div>This lesson doesn't have a video</div>
            )}

            {video?.token && (
                <HlsVideoPlayer token={video.token} title={lesson.title} />
            )}
        </div>
    );
}
