"use client";

import { FaPlay } from "react-icons/fa6";
import { Dialog, DialogBody, DialogHeader } from "@material-tailwind/react";

import { CourseDetail } from "@/libs/types/course";
import HlsVideoPlayer from "@/components/HlsVideoPlayer";
import usePreviewVideoModal from "../_hooks/usePreviewVideoModal";

export default function PreviewVideoModal({
    course,
}: {
    course: CourseDetail;
}) {
    const isOpen = usePreviewVideoModal((state) => state.isOpen);
    const close = usePreviewVideoModal((state) => state.close);

    return (
        <Dialog open={isOpen} handler={close}>
            <DialogHeader className="flex justify-between flex-nowrap">
                Preview video
                <button onClick={close} className="text-black font-semibold">
                    &#10539;
                </button>
            </DialogHeader>
            <DialogBody className="pb-10">
                {course.previewVideo &&
                course.previewVideo.status === "Processed" ? (
                    <HlsVideoPlayer
                        token={course.previewVideo.token || ""}
                        title={course.title}
                    />
                ) : (
                    <div className="text-black">
                        This course doesn't have a preview video
                    </div>
                )}
            </DialogBody>
        </Dialog>
    );
}

export function OpenPreviewVideoModalButton() {
    const open = usePreviewVideoModal((state) => state.open);

    return (
        <>
            <button
                title="Watch preview video"
                className="hover:scale-105 text-white"
                onClick={open}
            >
                <FaPlay size={52} />
            </button>
        </>
    );
}
