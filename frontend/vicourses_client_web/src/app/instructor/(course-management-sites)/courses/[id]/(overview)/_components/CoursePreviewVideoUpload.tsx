"use client";

import path from "path";
import { v4 as uuidv4 } from "uuid";
import { useState } from "react";
import { Typography } from "@material-tailwind/react";
import { useSession } from "next-auth/react";

import { VideoFile } from "@/libs/types/common";
import { CourseDetail } from "@/libs/types/course";
import { UploadResponse } from "@/libs/types/storage";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { updateCourse } from "@/services/api/course";
import { ChunkUpload } from "@/components/common";
import HlsVideoPlayer from "@/components/HlsVideoPlayer";

export default function CoursePreviewVideoUpload({
    initialCourse,
}: {
    initialCourse: CourseDetail;
}) {
    const [previewVideo, setPreviewVideo] = useState<VideoFile | null>(initialCourse.previewVideo);
    const { data: session } = useSession();

    const onComplete = async (result: UploadResponse) => {
        try {
            await updateCourse(
                initialCourse.id,
                {
                    previewVideoToken: result.token,
                },
                session?.accessToken || ""
            );
            
            setPreviewVideo({
                status: "BeingProcessed",
                originalFileName: "",
                duration: 0,
                token: null,
            });
        } catch (error: any) {}
    };

    const getVideoHtml = () => {
        if (!previewVideo) {
            return (
                <img
                    className="w-full max-h-60 object-cover aspect-video border border-gray-900"
                    src={DEFAULT_COURSE_THUMBNAIL_URL}
                    alt={initialCourse.title}
                />
            );
        }

        if (previewVideo.status === "BeingProcessed") {
            return (
                <div
                    className="relative w-full h-60 bg-center bg-cover border border-gray-900"
                    style={{
                        backgroundImage: `url('${DEFAULT_COURSE_THUMBNAIL_URL}')`,
                    }}
                >
                    <div className="absolute flex items-center justify-center w-full h-full text-white bg-black bg-opacity-70">
                        Video is being processed
                    </div>
                </div>
            );
        }

        if (previewVideo.status === "ProcessingFailed") {
            return (
                <div
                    className="relative w-full h-60 bg-center bg-cover border border-gray-900"
                    style={{
                        backgroundImage: `url('${DEFAULT_COURSE_THUMBNAIL_URL}')`,
                    }}
                >
                    <div className="absolute flex items-center justify-center w-full h-full text-error bg-black bg-opacity-70">
                        Video processing failed, try again
                    </div>
                </div>
            );
        }

        return (
            <HlsVideoPlayer
                title={initialCourse.title}
                token={previewVideo.token || ""}
                className="w-full max-h-60 object-cover aspect-video"
            />
        );
    };

    return (
        <div className="flex gap-3 flex-col md:flex-row">
            <div className="basis-2/5 max-h-60">{getVideoHtml()}</div>
            <div className="text-gray-900 basis-3/5">
                <Typography color="gray">
                    Your preview video is a quick and engaging way for students
                    to preview what they will learn in your course. Students
                    interested in your course are more likely to enroll if your
                    promotional video is well done.
                </Typography>
                <div className="my-2">
                    <ChunkUpload
                        extensions={[".mp4"]}
                        getFileId={(file) => {
                            const ext = path.extname(file.name);
                            return `vicourses-videos/${uuidv4()}${ext}`;
                        }}
                        onComplete={onComplete}
                    />
                </div>
            </div>
        </div>
    );
}
