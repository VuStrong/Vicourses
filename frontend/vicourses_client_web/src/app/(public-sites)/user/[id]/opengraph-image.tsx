import { ImageResponse } from "next/og";
import { auth } from "@/libs/auth";
import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";
import { getPublicProfile } from "@/services/api/user";

// Route segment config
export const runtime = "edge";

// Image metadata
export const alt = "Profile";
export const size = {
    width: 1200,
    height: 1200,
};

// export const contentType = "image/png";

export default async function Image({ params }: { params: { id: string } }) {
    const session = await auth();
    const profile = await getPublicProfile(params.id, session?.accessToken);

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
                    src={profile?.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                    alt={profile?.name}
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