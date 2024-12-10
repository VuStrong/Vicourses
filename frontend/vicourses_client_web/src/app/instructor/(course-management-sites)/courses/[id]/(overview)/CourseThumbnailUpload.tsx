"use client";

import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { Course } from "@/libs/types/course";
import { getFileExtension } from "@/libs/utils";
import { Input, Typography } from "@material-tailwind/react";
import { useState } from "react";

const validExtensions = ["jpeg", "jpg", "png"];

export default function CourseThumbnailUpload({
    course,
    onImageChange,
}: {
    course: Course;
    onImageChange: (file?: File) => void;
}) {
    const [error, setError] = useState<string>();
    const [tempUrl, setTempUrl] = useState<string>();

    return (
        <div className="flex gap-3 flex-col md:flex-row">
            <img
                className="basis-2/5 max-h-60 border aspect-square object-cover"
                src={
                    tempUrl
                        ? tempUrl
                        : course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL
                }
                alt={course.title}
            />
            <div className="text-gray-900 basis-3/5">
                <Typography color="gray" className="mb-2">
                    Upload course thumbnail here. Accept extensions:
                    .jpg,.jpeg,.png
                </Typography>
                <div>
                    <Input
                        type="file"
                        accept=".jpg,.jpeg,.png"
                        crossOrigin={undefined}
                        onChange={(e) => {
                            if (e.target.files?.[0]) {
                                const fileToUpdate = e.target.files[0];
                                const ext = getFileExtension(fileToUpdate);

                                if (!validExtensions.includes(ext)) {
                                    setError(`Invalid image extension.`);
                                    return;
                                }

                                setTempUrl(URL.createObjectURL(fileToUpdate));
                                setError(undefined);
                                onImageChange(fileToUpdate);
                            }
                        }}
                    />
                    {error && <Typography color="red">{error}</Typography>}
                </div>
            </div>
        </div>
    );
}
