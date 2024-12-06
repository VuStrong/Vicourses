"use client";

import { Category } from "@/libs/types/category";
import { getNavigationCategories } from "@/services/api/category";
import {
    Menu,
    MenuHandler,
    MenuItem,
    MenuList,
} from "@material-tailwind/react";
import Link from "next/link";
import { useEffect, useState } from "react";

export default function HeaderCategoriesMenu() {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [categories, setCategories] = useState<Category[]>([]);

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const result = await getNavigationCategories();
            setCategories(result);

            setIsLoading(false);
        })();
    }, []);

    if (isLoading) {
        return <div>Categories</div>;
    }

    return (
        <Menu placement="bottom-end" allowHover>
            <MenuHandler>
                <div className="cursor-pointer">Categories</div>
            </MenuHandler>
            <MenuList>
                {categories.map((category) => (
                    <Menu key={category.id} placement="right-start" allowHover offset={15}>
                        <MenuHandler className="flex items-center justify-between">
                            <Link href={`/category/${category.slug}`}>
                                <MenuItem className="flex justify-between">
                                    <div>{category.name}</div>
                                    <div>&#11208;</div>
                                </MenuItem>
                            </Link>
                        </MenuHandler>
                        <MenuList>
                            {category.subCategories &&
                                category.subCategories.map((subCategory) => (
                                    <Link
                                        key={subCategory.id}
                                        href={`/category/${subCategory.slug}`}
                                    >
                                        <MenuItem>{subCategory.name}</MenuItem>
                                    </Link>
                                ))}
                        </MenuList>
                    </Menu>
                ))}
            </MenuList>
        </Menu>
    );
}
