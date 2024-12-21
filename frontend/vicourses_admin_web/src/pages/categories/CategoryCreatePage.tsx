import { useEffect, useState } from "react";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";

import { Category } from "../../types/category";
import axiosInstance from "../../libs/axios";
import { Input, Select, Option } from "../../components/Forms";
import Button from "../../components/Button";

type FormValues = {
    name: string;
    parentId: string;
};

export default function CategoryCreatePage() {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>([]);
    const navigate = useNavigate();

    const { handleSubmit, control } = useForm<FormValues>({
        defaultValues: {
            name: "",
            parentId: "",
        },
    });

    const onSubmit: SubmitHandler<FormValues> = async (data) => {
        setIsLoading(true);

        try {
            await axiosInstance.post("/api/cs/v1/categories", {
                name: data.name,
                parentId: data.parentId || undefined,
            });

            toast.success(`Category ${data.name} created`);
            navigate("/categories");
        } catch (error: any) {
            toast.error(error.response?.data?.message || "Error");
        }

        setIsLoading(false);
    };

    useEffect(() => {
        (async () => {
            const result = await axiosInstance.get<Category[]>(
                "/api/cs/v1/categories",
                {
                    params: { parentId: "null" },
                },
            );

            setCategories(result.data);
        })();
    }, []);

    return (
        <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
            <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
                <h3 className="font-medium text-black dark:text-white">
                    Create category
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
                    <label className="mb-3 block text-black dark:text-white">
                        Parent category
                    </label>
                    <Controller
                        name="parentId"
                        control={control}
                        render={({ field }) => (
                            <Select
                                value={field.value}
                                onChange={field.onChange}
                                disabled={isLoading}
                            >
                                <Option value="" disabled>
                                    Select parent
                                </Option>
                                {categories.map((category) => (
                                    <Option
                                        key={category.id}
                                        value={category.id}
                                    >
                                        {category.name}
                                    </Option>
                                ))}
                            </Select>
                        )}
                    />
                </div>
                <div>
                    <Button type="submit" size="sm" loading={isLoading}>
                        Create
                    </Button>
                </div>
            </form>
        </div>
    );
}
