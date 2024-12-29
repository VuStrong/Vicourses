import { useState } from "react";
import { FaPlay } from "react-icons/fa";

import { DEFAULT_COURSE_IMAGE_URL } from "../../libs/contants";
import { CourseDetail } from "../../types/course";
import { Modal, ModalBody, ModalHeader } from "../Modal";
import HlsVideoPlayer from "../HlsVideoPlayer";

export default function CoursePreviewVideo({
    course,
}: {
    course: CourseDetail;
}) {
    const [modalOpen, setModalOpen] = useState<boolean>(false);

    return (
        <>
            <div
                className="relative w-full border border-gray-700"
                style={{
                    aspectRatio: "calc(1 / 0.5625)",
                }}
            >
                <img
                    className="w-full h-full"
                    src={course.thumbnailUrl || DEFAULT_COURSE_IMAGE_URL}
                    alt={course.title}
                />
                <div className="absolute w-full h-full bg-black/50 top-0 left-0 flex justify-center items-center">
                    <button
                        title="Watch preview video"
                        onClick={() => setModalOpen(true)}
                        className="hover:scale-110"
                    >
                        <FaPlay size={42} color="#ffffff" />
                    </button>
                </div>
            </div>

            <Modal open={modalOpen} onClose={() => setModalOpen(false)}>
                <ModalHeader className="flex justify-between flex-nowrap">
                    Preview video
                    <button
                        onClick={() => setModalOpen(false)}
                        className="font-semibold"
                    >
                        &#10539;
                    </button>
                </ModalHeader>
                <ModalBody>
                    {course.previewVideo &&
                    course.previewVideo.status === "Processed" ? (
                        <HlsVideoPlayer 
                            token={course.previewVideo.token || ""}
                            title={course.title}
                        />
                    ) : (
                        <div>This course doesn't have a preview video</div>
                    )}
                </ModalBody>
            </Modal>
        </>
    );
}
