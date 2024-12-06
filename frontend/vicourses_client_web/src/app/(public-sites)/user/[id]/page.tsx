import { Metadata } from "next";
import { notFound } from "next/navigation";
import { MdLink } from "react-icons/md";
import { FaYoutube, FaFacebook, FaLinkedin } from "react-icons/fa";
import { auth } from "@/libs/auth";
import { getPublicProfile } from "@/services/api/user";
import { DEFAULT_USER_AVATAR_URL } from "@/libs/constants";

export async function generateMetadata({
    params,
}: {
    params: { id: string };
}): Promise<Metadata> {
    const session = await auth();
    const profile = await getPublicProfile(params.id, session?.accessToken);

    if (!profile) notFound();

    return {
        title: `${profile.name} | Vicourses`,
        description: profile.headline || "",
        openGraph: {
            title: `${profile.name} | Vicourses`,
            description: profile.headline || "",
        },
    };
}

export default async function PublicProfilePage({
    params,
}: {
    params: { id: string };
}) {
    const session = await auth();
    const profile = await getPublicProfile(params.id, session?.accessToken);

    if (!profile) notFound();

    return (
        <div className="max-w-[61.2rem] mx-auto p-5 flex md:flex-row flex-col gap-10 bg-white my-5 rounded-lg shadow-2xl">
            <div className="flex flex-col gap-5">
                <img
                    className="w-52 h-52 rounded-full object-cover object-center"
                    alt={profile.name}
                    src={profile.thumbnailUrl || DEFAULT_USER_AVATAR_URL}
                />
                {profile.websiteUrl && (
                    <a 
                        href={profile.websiteUrl}
                        target="_blank"
                        className="border border-gray-800 py-2 px-5 bg-transparent flex items-center justify-center gap-2 text-black font-bold"
                    >
                        <MdLink size={15} />
                        Website
                    </a>
                )}
                {profile.youtubeUrl && (
                    <a 
                        href={profile.youtubeUrl}
                        target="_blank"
                        className="border border-gray-800 py-2 px-5 bg-transparent flex items-center justify-center gap-2 text-black font-bold"
                    >
                        <FaYoutube size={15} />
                        Youtube
                    </a>
                )}
                {profile.facebookUrl && (
                    <a 
                        href={profile.facebookUrl}
                        target="_blank"
                        className="border border-gray-800 py-2 px-5 bg-transparent flex items-center justify-center gap-2 text-black font-bold"
                    >
                        <FaFacebook size={15} />
                        Facebook
                    </a>
                )}
                {profile.linkedInUrl && (
                    <a 
                        href={profile.linkedInUrl}
                        target="_blank"
                        className="border border-gray-800 py-2 px-5 bg-transparent flex items-center justify-center gap-2 text-black font-bold"
                    >
                        <FaLinkedin size={15} />
                        LinkedIn
                    </a>
                )}
            </div>
            <div>
                <div className="text-gray-700 text-base uppercase">{profile.role}</div>
                <h1 className="text-black font-semibold text-3xl">{profile.name}</h1>
                <h2 className="text-black font-semibold text-xl">{profile.headline || ""}</h2>

                <div className="flex my-5">
                    <div>
                        <div className="text-gray-600 font-bold">Total students</div>
                        <div className="text-black font-bold text-3xl">{profile.totalEnrollmentCount}</div>
                    </div>
                </div>

                {profile.description && (
                    <div>
                        <h2 className="text-black font-semibold text-xl">About me</h2>
                        <div>{profile.description}</div>
                    </div>
                )}
            </div>
        </div>
    );
}
