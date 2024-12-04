"use client";

import { useSession } from "next-auth/react";
import { ChangeEvent, useEffect, useState } from "react";
import { v4 as uuidv4 } from "uuid";
import S3ChunkUploader from "@/libs/s3-chunk-uploader";
import { getFileExtension } from "@/libs/utils";

const allowedExtensions = ["mp4", "mkv"];

export default function VideoUpload() {
    const [percent, setPercent] = useState<number>(0);
    const [file, setFile] = useState<File>();
    const [uploader, setUploader] = useState<S3ChunkUploader>();
    const [error, setError] = useState<string>();
    const session = useSession();

    useEffect(() => {
        if (file) {
            const fileId = `vicourses-videos/${uuidv4()}.${getFileExtension(
                file
            )}`;

            const uploader = new S3ChunkUploader({
                file,
                fileId,
                accessToken: session.data?.accessToken ?? "",
                onError: () => {
                    setFile(undefined);
                    setPercent(0);
                    setUploader(undefined);
                    setError("Error when uploading, try again.");
                },
                onProgress: (percentage) => {
                    setPercent(percentage);
                },
                onComplete(fileId) {
                    console.log("All done: " + fileId);
                },
            });
            setUploader(uploader);

            uploader.start();
            setError(undefined);
        }
    }, [file]);

    useEffect(() => {
        if (uploader && session.data?.accessToken) {
            uploader.accessToken = session.data.accessToken;
        }
    }, [session.data?.accessToken]);

    const onFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        const file = e.target?.files?.[0];

        if (!file) return;

        if (file.size >= 1024 * 1024 * 1024 * 2) {
            setError("Video size cannot larger than 2GB");
            return;
        }
        if (!allowedExtensions.includes(getFileExtension(file))) {
            setError("Only allow .mp4, .mkv video extension");
            return;
        }

        setFile(file);
    }

    const handleAbort = () => {
        if (uploader) {
            uploader.abort();
            setFile(undefined);
            setPercent(0);
            setUploader(undefined);
        }
    };

    return (
        <div className="w-full bg-white rounded-lg relative">
            {file ? (
                <div className="p-2 border-gray-300 border-2 rounded-lg">
                    <div className="mb-2 flex justify-between items-center">
                        <div className="flex items-center gap-x-3 flex-1">
                            <span className="size-8 flex justify-center items-center border border-gray-200 text-gray-500 rounded-lg">
                                <svg
                                    className="shrink-0 size-5"
                                    xmlns="http://www.w3.org/2000/svg"
                                    width="24"
                                    height="24"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="currentColor"
                                >
                                    <path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"></path>
                                    <polyline points="17 8 12 3 7 8"></polyline>
                                    <line x1="12" x2="12" y1="3" y2="15"></line>
                                </svg>
                            </span>
                            <div>
                                <p className="text-sm font-medium text-gray-800 line-clamp-2">
                                    {file.name}
                                </p>
                            </div>
                        </div>
                        <div className="inline-flex items-center gap-x-2">
                            <button
                                onClick={handleAbort}
                                title="Cancel"
                                type="button"
                                className="relative text-gray-500 hover:text-gray-800 focus:outline-none focus:text-gray-800 disabled:opacity-50 disabled:pointer-events-none"
                            >
                                <svg
                                    className="shrink-0 size-4"
                                    xmlns="http://www.w3.org/2000/svg"
                                    width="24"
                                    height="24"
                                    viewBox="0 0 24 24"
                                    fill="none"
                                    stroke="currentColor"
                                >
                                    <path d="M3 6h18"></path>
                                    <path d="M19 6v14c0 1-1 2-2 2H7c-1 0-2-1-2-2V6"></path>
                                    <path d="M8 6V4c0-1 1-2 2-2h4c1 0 2 1 2 2v2"></path>
                                    <line
                                        x1="10"
                                        x2="10"
                                        y1="11"
                                        y2="17"
                                    ></line>
                                    <line
                                        x1="14"
                                        x2="14"
                                        y1="11"
                                        y2="17"
                                    ></line>
                                </svg>
                                <span className="sr-only">Delete</span>
                            </button>
                        </div>
                    </div>

                    <div className="flex items-center gap-x-3 whitespace-nowrap">
                        <div className="flex w-full h-2 bg-gray-200 rounded-full overflow-hidden">
                            <div
                                style={{
                                    width: `${percent}%`,
                                }}
                                className="flex flex-col justify-center rounded-full overflow-hidden bg-primary text-xs text-white text-center whitespace-nowrap transition duration-500"
                            ></div>
                        </div>
                        <div className="w-6 text-end">
                            <span className="text-sm text-gray-800">
                                {percent}%
                            </span>
                        </div>
                    </div>
                </div>
            ) : (
                <input
                    className="block w-full text-lg text-gray-900 border border-gray-300 rounded-lg cursor-pointer focus:outline-none"
                    type="file"
                    onChange={onFileChange}
                />
            )}

            {error && <div className="text-error absolute bot-0">{error}</div>}
        </div>
    );
}
