"use client";

import path from "path";
import { Session } from "next-auth";
import { FaPlay } from "react-icons/fa";
import { v4 as uuidv4 } from "uuid";

import { ChunkUpload } from "@/components/common";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { LessonVideo } from "@/libs/types/lesson";
import { formatLength } from "@/libs/utils";
import { UploadResponse } from "@/libs/types/storage";
import { updateLesson } from "@/services/api/course-lesson";

export default function LessonVideoUpload({
    lessonId,
    video,
    session,
    onVideoChanged,
}: {
    lessonId: string;
    video: LessonVideo | null;
    session: Session | null;
    onVideoChanged: (video: LessonVideo) => void;
}) {
    const onComplete = async (result: UploadResponse) => {
        try {
            const updatedLesson = await updateLesson(
                lessonId,
                {
                    videoToken: result.token,
                },
                session?.accessToken || ""
            );

            if (updatedLesson.video) {
                onVideoChanged(updatedLesson.video);
            }
        } catch (error: any) {}
    };

    return (
        <>
            <div className="flex gap-2 mb-3">
                <div
                    className={`min-w-28 h-16 bg-cover border border-gray-700 flex justify-center items-center bg-black`}
                    style={{
                        backgroundImage:
                            !video || video.status !== "Processed"
                                ? `url('${DEFAULT_COURSE_THUMBNAIL_URL}')`
                                : undefined,
                    }}
                >
                    {video && video.status === "Processed" && (
                        <div className="text-white">
                            <FaPlay size={26} />
                        </div>
                    )}
                </div>
                <div className="flex-grow min-w-0">
                    {video ? (
                        <>
                            <div className="text-black font-semibold line-clamp-1">
                                {video.originalFileName}
                            </div>
                            <div className="text-gray-900 text-sm">
                                {formatLength(video.duration)}
                            </div>
                            {video.status === "Processed" && (
                                <div className="text-primary font-semibold text-sm">
                                    Processed
                                </div>
                            )}
                            {video.status === "BeingProcessed" && (
                                <div className="text-yellow-900 font-semibold text-sm">
                                    Being processed...
                                </div>
                            )}
                            {video.status === "ProcessingFailed" && (
                                <div className="text-error font-semibold text-sm">
                                    Processing failed, try again
                                </div>
                            )}
                        </>
                    ) : (
                        <div>No video uploaded</div>
                    )}
                </div>
            </div>
            <div>
                <ChunkUpload
                    extensions={[".mp4"]}
                    getFileId={(file) => {
                        const ext = path.extname(file.name);
                        return `vicourses-videos/${uuidv4()}${ext}`;
                    }}
                    onComplete={onComplete}
                />
            </div>
        </>
    );
}
