import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import toast from "react-hot-toast";

import { Category } from "../../types/category";
import axiosInstance from "../../libs/axios";
import Loader from "../../components/Loader";
import Button from "../../components/Button";
import { Input, Select, Option } from "../../components/Forms";
import {
    Modal,
    ModalBody,
    ModalFooter,
    ModalHeader,
} from "../../components/Modal";

type FormValues = {
    name: string;
};

export default function CategoryEditPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isEditing, setIsEditing] = useState<boolean>(false);
    const [modalOpen, setModalOpen] = useState<boolean>(false);
    const [category, setCategory] = useState<Category>();
    const [categories, setCategories] = useState<Category[]>([]);

    const params = useParams();
    const navigate = useNavigate();

    const {
        handleSubmit,
        control,
        reset,
        formState: { isDirty },
    } = useForm<FormValues>({
        defaultValues: {
            name: "",
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        if (isEditing || !category) return;

        setIsEditing(true);

        try {
            await axiosInstance.patch(`/api/cs/v1/categories/${category.id}`, {
                name: data.name,
            });

            toast.success(`Category ${data.name} edited`);
            reset(data);
        } catch (error: any) {
            toast.error(error.response?.data?.message || "Error");
        }

        setIsEditing(false);
    };

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<Category>(`/api/cs/v1/categories/${params.slug}`)
            .then((response) => {
                setCategory(response.data);
                reset({
                    name: response.data.name,
                });
                setIsLoading(false);
            })
            .catch((error) => {
                if (error.response?.status === 404) {
                    setIsLoading(false);
                }
            });

        axiosInstance
            .get<Category[]>("/api/cs/v1/categories", {
                params: { parentId: "null" },
            })
            .then((response) => setCategories(response.data));
    }, [params.slug]);

    const handleDelete = async () => {
        if (!category) return;

        try {
            await axiosInstance.delete(`/api/cs/v1/categories/${category.id}`);

            navigate("/categories", { replace: true });
            toast.success("Category deleted");
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

                {!isLoading && !category && (
                    <div className="text-center">Category not found</div>
                )}

                {!isLoading && category && (
                    <>
                        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                            <h3 className="font-medium text-black dark:text-white">
                                Category
                            </h3>
                        </div>
                        <form
                            className="flex flex-col gap-5.5 p-6.5"
                            onSubmit={handleSubmit(onSubmit)}
                        >
                            <div>
                                <label
                                    htmlFor="name"
                                    className="mb-3 block text-black dark:text-white"
                                >
                                    Name
                                </label>
                                <Controller
                                    name="name"
                                    control={control}
                                    rules={{
                                        required: {
                                            value: true,
                                            message: "Enter category name.",
                                        },
                                        minLength: {
                                            value: 2,
                                            message:
                                                "Name must be between 2 and 40 characters",
                                        },
                                        maxLength: {
                                            value: 40,
                                            message:
                                                "Name must be between 2 and 40 characters",
                                        },
                                    }}
                                    render={({ field, fieldState }) => (
                                        <>
                                            <Input
                                                value={field.value}
                                                onChange={field.onChange}
                                                id="name"
                                                placeholder="Enter category Name"
                                                error={!!fieldState.error}
                                                disabled={isLoading}
                                            />
                                            {fieldState.error && (
                                                <p className="text-red-700">
                                                    {fieldState.error.message}
                                                </p>
                                            )}
                                        </>
                                    )}
                                />
                            </div>
                            <div>
                                <div className="mb-3 block text-black dark:text-white">
                                    Parent category
                                </div>
                                <Select
                                    value={category.parentId || ""}
                                    disabled
                                >
                                    <Option value="">None</Option>
                                    {categories.map((c) => (
                                        <Option key={c.id} value={c.id}>
                                            {c.name}
                                        </Option>
                                    ))}
                                </Select>
                            </div>
                            <div>
                                <div>
                                    Created At:{" "}
                                    {new Date(
                                        category.createdAt,
                                    ).toLocaleDateString()}
                                </div>
                                <div>
                                    Updated At:{" "}
                                    {new Date(
                                        category.updatedAt,
                                    ).toLocaleDateString()}
                                </div>
                            </div>
                            <div className="flex gap-3 flex-wrap">
                                <Button
                                    type="submit"
                                    size="sm"
                                    disabled={!isDirty || isEditing}
                                    loading={isEditing}
                                >
                                    Edit
                                </Button>
                                <Button
                                    type="button"
                                    size="sm"
                                    disabled={isEditing}
                                    onClick={() => setModalOpen(true)}
                                    variant="text"
                                    className="text-red-700"
                                >
                                    Delete
                                </Button>
                            </div>
                        </form>
                    </>
                )}
            </div>
            <Modal open={modalOpen} onClose={() => setModalOpen(false)}>
                <ModalHeader>Delete this category?</ModalHeader>
                <ModalBody>
                    Note that if this category is in used by other courses or
                    categories, it cannot be deleted
                </ModalBody>
                <ModalFooter>
                    <Button onClick={() => setModalOpen(false)} variant="text">
                        Cancel
                    </Button>
                    <Button onClick={handleDelete}>Confirm</Button>
                </ModalFooter>
            </Modal>
        </>
    );
}
