import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { FaFacebook, FaLinkedin, FaYoutube, FaCheck } from "react-icons/fa";
import { MdLink, MdLock, MdOutlineClose } from "react-icons/md";

import { User } from "../../types/user";
import axiosInstance from "../../libs/axios";
import { DEFAULT_USER_AVATAR_URL } from "../../libs/contants";
import Loader from "../../components/Loader";
import Button from "../../components/Button";
import toast from "react-hot-toast";
import {
    Modal,
    ModalBody,
    ModalFooter,
    ModalHeader,
} from "../../components/Modal";
import { Input } from "../../components/Forms";

const selectFields =
    "id,createdAt,name,email,emailConfirmed,lockoutEnd,role,thumbnailUrl,headline,description,websiteUrl,youtubeUrl,facebookUrl,linkedInUrl,paypalAccount";

export default function UserDetailPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [user, setUser] = useState<User | null>(null);
    const [lockModalOpen, setLockModalOpen] = useState<boolean>(false);

    const params = useParams();

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<User>(`/api/us/v1/users/${params.id}`, {
                params: {
                    fields: selectFields,
                },
            })
            .then((response) => {
                setUser(response.data);
                setIsLoading(false);
            })
            .catch((error) => {
                if (error?.response?.status === 404) {
                    setIsLoading(false);
                }
            });
    }, [params.id]);

    const handLockUser = async (days: number) => {
        if (!user) return;

        try {
            await axiosInstance.patch(`/api/us/v1/users/${user.id}/lock`, {
                days,
            });

            let lockTo = null;
            if (days > 0) {
                lockTo = new Date();
                lockTo.setDate(lockTo.getDate() + days);
            }

            setUser({
                ...user,
                lockoutEnd: lockTo ? lockTo.toISOString() : null,
            });
            setLockModalOpen(false);
            toast.success("Success");
        } catch (error: any) {
            toast.error(error.response?.data?.message || "Error");
        }
    };

    return (
        <>
            <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
                {isLoading && (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                )}

                {!isLoading && !user && (
                    <div className="text-center">User not found</div>
                )}

                {!isLoading && user && (
                    <div className="overflow-hidden rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark p-5">
                        <div className="md:flex mb-5">
                            <div className="w-40 h-40 flex-shrink-0">
                                <img
                                    className="w-full h-full rounded-full object-cover border"
                                    src={
                                        user.thumbnailUrl ??
                                        DEFAULT_USER_AVATAR_URL
                                    }
                                    alt={user.name}
                                />
                            </div>

                            <div className="p-4 flex flex-col justify-between leading-normal">
                                <div className="md:mb-3">
                                    <p className="text-sm uppercase flex items-center">
                                        {user.role}
                                    </p>
                                    <div className="font-bold text-3xl mb-2">
                                        {user.name}
                                    </div>
                                    {user.headline && (
                                        <div className="font-semibold">
                                            {user.headline}
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>

                        <div className="mb-5">
                            {user.websiteUrl && (
                                <a
                                    href={user.websiteUrl}
                                    target="_blank"
                                    className="border dark:border-gray-600 py-2 px-5 flex items-center justify-center gap-2 font-bold w-full md:max-w-[200px]"
                                >
                                    <MdLink size={15} />
                                    Website
                                </a>
                            )}
                            {user.youtubeUrl && (
                                <a
                                    href={user.youtubeUrl}
                                    target="_blank"
                                    className="border dark:border-gray-600 py-2 px-5 flex items-center justify-center gap-2 font-bold w-full md:max-w-[200px]"
                                >
                                    <FaYoutube size={15} />
                                    Youtube
                                </a>
                            )}
                            {user.facebookUrl && (
                                <a
                                    href={user.facebookUrl}
                                    target="_blank"
                                    className="border dark:border-gray-600 py-2 px-5 flex items-center justify-center gap-2 font-bold w-full md:max-w-[200px]"
                                >
                                    <FaFacebook size={15} />
                                    Facebook
                                </a>
                            )}
                            {user.linkedInUrl && (
                                <a
                                    href={user.linkedInUrl}
                                    target="_blank"
                                    className="border dark:border-gray-600 py-2 px-5 flex items-center justify-center gap-2 font-bold w-full md:max-w-[200px]"
                                >
                                    <FaLinkedin size={15} />
                                    LinkedIn
                                </a>
                            )}
                        </div>

                        <div className="mb-5">
                            <div className="md:flex gap-3 items-center">
                                <Button
                                    variant="text"
                                    className="border text-red-600 dark:border-gray-600 w-full md:w-fit"
                                    onClick={() => setLockModalOpen(true)}
                                >
                                    <MdLock />
                                    Lock this user
                                </Button>
                                <div className="text-center text-sm">
                                    {user.lockoutEnd &&
                                        `This user is locked until ${user.lockoutEnd}`}
                                </div>
                            </div>
                        </div>

                        <div className="mb-5">
                            <div>
                                <span className="font-semibold">Email:</span>{" "}
                                {user.email}
                            </div>
                            <div className="flex gap-2 items-center">
                                <span className="font-semibold">
                                    Email confirmed:
                                </span>{" "}
                                {user.emailConfirmed ? (
                                    <div className="text-green-500">
                                        <FaCheck />
                                    </div>
                                ) : (
                                    <div className="text-red-500">
                                        <MdOutlineClose />
                                    </div>
                                )}
                            </div>
                            <div>
                                <span className="font-semibold">
                                    Created At:
                                </span>{" "}
                                {new Date(user.createdAt).toLocaleDateString()}
                            </div>
                            {user.paypalAccount && (
                                <div>
                                    <span className="font-semibold">
                                        Paypal payouts account:
                                    </span>{" "}
                                    {user.paypalAccount.email}
                                </div>
                            )}
                        </div>

                        {user.description && (
                            <div>
                                <p className="font-semibold">Description:</p>
                                <div
                                    className="mb-3"
                                    dangerouslySetInnerHTML={{
                                        __html: user.description,
                                    }}
                                />
                            </div>
                        )}
                    </div>
                )}
            </div>

            <LockUserModal
                open={lockModalOpen}
                onClose={() => setLockModalOpen(false)}
                submit={handLockUser}
            />
        </>
    );
}

function LockUserModal({
    open,
    onClose,
    submit,
}: {
    open: boolean;
    onClose: () => void;
    submit: (days: number) => Promise<void>;
}) {
    const [days, setDays] = useState<number>(0);
    const [isSubmitting, setIsSubmitting] = useState<boolean>(false);

    const handleSubmit = async () => {
        setIsSubmitting(true);
        await submit(days);
        setIsSubmitting(false);
    };

    return (
        <Modal open={open} onClose={onClose}>
            <ModalHeader>Lock user</ModalHeader>
            <ModalBody>
                <label htmlFor="days" className="text-sm">
                    Number of days to lock, if the value is less than or equal
                    to 0, user will be unlocked
                </label>
                <Input
                    id="days"
                    type="number"
                    disabled={isSubmitting}
                    value={days}
                    onChange={(e) => setDays(+e.target.value || 0)}
                />
            </ModalBody>
            <ModalFooter>
                <Button onClick={onClose} variant="text">
                    Cancel
                </Button>
                <Button onClick={handleSubmit} loading={isSubmitting}>
                    Confirm
                </Button>
            </ModalFooter>
        </Modal>
    );
}
