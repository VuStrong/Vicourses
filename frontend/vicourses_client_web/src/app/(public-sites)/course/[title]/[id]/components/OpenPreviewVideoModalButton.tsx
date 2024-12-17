"use client";

import { useState } from "react";
import { FaPlay } from "react-icons/fa6";
import { Dialog, DialogBody, DialogHeader } from "@material-tailwind/react";
import { MediaPlayer, MediaProvider } from "@vidstack/react";
import {
    defaultLayoutIcons,
    DefaultVideoLayout,
} from "@vidstack/react/player/layouts/default";
import "@vidstack/react/player/styles/default/theme.css";
import "@vidstack/react/player/styles/default/layouts/video.css";

import { CourseDetail } from "@/libs/types/course";

export default function OpenPreviewVideoModalButton({
    course,
}: {
    course: CourseDetail;
}) {
    const [open, setOpen] = useState<boolean>(false);

    return (
        <>
            <button
                title="Watch preview video"
                className="hover:scale-105 text-white"
                onClick={() => setOpen(true)}
            >
                <FaPlay size={52} />
            </button>
            <Dialog open={open} handler={() => setOpen(false)}>
                <DialogHeader className="flex justify-between flex-nowrap">
                    Preview video
                    <button
                        onClick={() => setOpen(false)}
                        className="text-black font-semibold"
                    >
                        &#10539;
                    </button>
                </DialogHeader>
                <DialogBody className="pb-10">
                    {course.previewVideo &&
                    course.previewVideo.status === "Processed" ? (
                        <div>
                            <MediaPlayer
                                autoPlay={false}
                                title={course.title}
                                src={course.previewVideo.streamFileUrl || ""}
                                className="w-full object-cover aspect-video"
                            >
                                <MediaProvider />
                                <DefaultVideoLayout
                                    thumbnails="https://files.vidstack.io/sprite-fight/thumbnails.vtt"
                                    icons={defaultLayoutIcons}
                                />
                            </MediaPlayer>
                        </div>
                    ) : (
                        <div className="text-black">
                            This course doesn't have a preview video
                        </div>
                    )}
                </DialogBody>
            </Dialog>
        </>
    );
}
