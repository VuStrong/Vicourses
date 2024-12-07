import { ImageResponse } from "next/og";
import { auth } from "@/libs/auth";
import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { getCourseById } from "@/services/api/course";

// Route segment config
export const runtime = "edge";

// Image metadata
export const alt = "Course";
export const size = {
    width: 1200,
    height: 1200,
};

// export const contentType = "image/png";

export default async function Image({ params }: { params: { id: string } }) {
    const session = await auth();
    const course = await getCourseById(params.id, session?.accessToken);

    return new ImageResponse(
        (
            <div
                style={{
                    fontSize: 128,
                    background: "white",
                    width: "100%",
                    height: "100%",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                }}
            >
                <img
                    src={course?.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL}
                    alt={course?.title}
                    style={{
                        width: "100%",
                        height: "100%",
                    }}
                />
            </div>
        ),
        {
            ...size,
        },
    );
}