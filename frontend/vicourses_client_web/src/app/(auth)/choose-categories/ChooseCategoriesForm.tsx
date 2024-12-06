"use client";

import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { Button } from "@material-tailwind/react";
import { Category } from "@/libs/types/category";
import { Loader } from "../../../components/common";
import { getCategories } from "@/services/api/category";
import { updateProfile } from "@/services/api/user";

export default function ChooseCategoriesForm() {
    const { data: session } = useSession();
    const [isLoadingCategories, setIsLoadingCategories] = useState<boolean>(false);
    const [isUpdating, setIsUpdating] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>([]);
    const [selectedIds, setSelectedIds] = useState<string[]>([]);
    const router = useRouter();

    useEffect(() => {
        (async () => {
            setIsLoadingCategories(true);

            const result = await getCategories();
            setCategories(result);

            setIsLoadingCategories(false);
        })();
    }, []);

    const onSelectedCategory = (categoryId: string) => {
        if (selectedIds.includes(categoryId)) {
            setSelectedIds(selectedIds.filter((id) => id !== categoryId));
        } else {
            setSelectedIds([...selectedIds, categoryId]);
        }
    };

    const onConfirm = async () => {
        setIsUpdating(true);

        await updateProfile({
            categoryIds: selectedIds.join(","),
        }, session?.accessToken || "").catch(() => undefined);
        
        router.push("/");
        setIsUpdating(false);
    }

    return (
        <div className="lg:h-auto md:h-auto border-0 rounded-lg shadow-2xl flex flex-col w-full bg-white outline-none focus:outline-none">
            <div className="relative p-6 flex-auto">
                <div className="flex flex-col gap-4">
                    <div className="text-start">
                        <div className="text-xl md:text-2xl font-bold text-primary mb-2">
                            What topic do you want to learn about?
                        </div>
                    </div>
                    {isLoadingCategories ? (
                        <div className="flex justify-center">
                            <Loader />
                        </div>
                    ) : (
                        <div className="flex flex-wrap gap-4">
                            {categories.map((category) => (
                                <Button
                                    key={category.id}
                                    className={`${
                                        selectedIds.includes(category.id)
                                            ? "bg-primary text-white"
                                            : "bg-transparent text-gray-900"
                                        }`
                                    }
                                    onClick={() =>
                                        onSelectedCategory(category.id)
                                    }
                                >
                                    {category.name}
                                </Button>
                            ))}
                        </div>
                    )}
                </div>
                <div className="flex flex-col gap-2 p-6">
                    <div className="flex flex-col items-center flex-wrap gap-4 w-full">
                        <Button
                            fullWidth
                            loading={isUpdating}
                            onClick={onConfirm}
                            className="bg-primary"
                        >
                            Confirm
                        </Button>
                    </div>
                </div>
            </div>
        </div>
    );
}
