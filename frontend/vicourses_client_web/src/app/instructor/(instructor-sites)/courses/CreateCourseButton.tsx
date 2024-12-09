"use client";

import { Button } from "@material-tailwind/react";
import { useState } from "react";
import CreateCourseModal from "./CreateCourseModal";

export default function CreateCourseButton() {
    const [modalOpen, setModalOpen] = useState<boolean>(false);

    return (
        <>
            <CreateCourseModal
                open={modalOpen}
                onClose={() => setModalOpen(false)}
            />

            <Button
                className="bg-primary rounded-none"
                onClick={() => setModalOpen(true)}
            >
                New course
            </Button>
        </>
    );
}
