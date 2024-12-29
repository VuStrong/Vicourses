"use client"

import HlsVideoPlayer from "@/components/HlsVideoPlayer";
import { Lesson } from "@/libs/types/lesson";

export default function LessonVideo({
    lesson,
}: {
    lesson: Lesson,
}) {
    if (!lesson.video?.token) {
        return (
            <div className="w-full bg-black flex justify-center items-center max-h-96 aspect-video">
                <div className="text-white">No video</div>
            </div>
        )
    }

    return (
        <div className="w-full max-h-96 aspect-video">
            <HlsVideoPlayer 
                title={lesson.title}
                token={lesson.video.token}
            />
        </div>
    )
}