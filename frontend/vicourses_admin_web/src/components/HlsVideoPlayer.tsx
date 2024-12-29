"use client";

import { useEffect, useState } from "react";
import {
    isHLSProvider,
    MediaPlayer,
    MediaProvider,
    MediaProviderAdapter,
} from "@vidstack/react";
import {
    defaultLayoutIcons,
    DefaultVideoLayout,
} from "@vidstack/react/player/layouts/default";
import "@vidstack/react/player/styles/default/theme.css";
import "@vidstack/react/player/styles/default/layouts/video.css";
import axiosInstance from "../libs/axios";

export default function HlsVideoPlayer({
    token,
    title,
    className,
}: {
    token: string;
    title?: string;
    className?: string;
}) {
    const [src, setSrc] = useState<string>("");
    const [params, setParams] = useState<string>("");

    const onProviderChange = (provider: MediaProviderAdapter | null) => {
        if (isHLSProvider(provider)) {
            provider.config = {
                xhrSetup(xhr, url) {
                    xhr.open("GET", `${url}?${params}`, true);
                },
            };
        }
    };

    useEffect(() => {
        if (!token) return;

        (async () => {
            const response = await axiosInstance.get("/api/sts/v1/hls-manifest-url", {
                params: {
                    token,
                }
            });

            setSrc(response.data.manifestFileUrl);
            setParams(response.data.params);
        })();
    }, [token]);

    return (
        <MediaPlayer
            onProviderChange={onProviderChange}
            autoPlay={false}
            title={title}
            src={src}
            aspectRatio="16/9"
            className={`h-full w-full ${className || ""}`}
        >
            <MediaProvider />
            <DefaultVideoLayout
                thumbnails="https://files.vidstack.io/sprite-fight/thumbnails.vtt"
                icons={defaultLayoutIcons}
            />
        </MediaPlayer>
    );
}
