"use client"

import { MediaPlayer, MediaProvider } from "@vidstack/react";
import {
    defaultLayoutIcons,
    DefaultVideoLayout,
} from "@vidstack/react/player/layouts/default";
import "@vidstack/react/player/styles/default/theme.css";
import "@vidstack/react/player/styles/default/layouts/video.css";

import { Lesson } from "@/libs/types/lesson";

export default function LessonVideo({
    lesson,
}: {
    lesson: Lesson,
}) {
    if (!lesson.video?.streamFileUrl) {
        return (
            <div className="w-full bg-black flex justify-center items-center max-h-96 aspect-video">
                <div className="text-white">No video</div>
            </div>
        )
    }

    return (
        <div className="w-full max-h-96 aspect-video">
            <MediaPlayer
                autoPlay={false}
                title={lesson.title}
                src={lesson.video.streamFileUrl}
                aspectRatio="16/9"
                className="h-full w-full"
            >
                <MediaProvider />
                <DefaultVideoLayout
                    thumbnails="https://files.vidstack.io/sprite-fight/thumbnails.vtt"
                    icons={defaultLayoutIcons}
                />
            </MediaPlayer>
        </div>
    )
}