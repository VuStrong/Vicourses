"use client";

import { useEffect, useState } from "react";
import {
    Accordion,
    AccordionBody,
    AccordionHeader,
    Drawer,
    Typography,
} from "@material-tailwind/react";
import { Category } from "@/libs/types/category";
import { getNavigationCategories } from "@/services/api/category";
import Link from "next/link";

export default function MobileSidebarDrawer({
    open,
    onClose,
}: {
    open: boolean;
    onClose: () => void;
}) {
    return (
        <Drawer
            open={open}
            onClose={onClose}
            className="p-4 overflow-y-auto"
            overlayProps={{
                className: "fixed",
            }}
        >
            <div className="border-b border-gray-500 pb-3">
                <div className="text-sm text-gray-800 mb-3">Categories</div>

                <SidebarCategoriesList onTapItem={onClose} />
            </div>
        </Drawer>
    );
}

function SidebarCategoriesList({ onTapItem }: { onTapItem?: () => void }) {
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [open, setOpen] = useState<number>(-1);
    const [categories, setCategories] = useState<Category[]>([]);

    const handleOpen = (value: number) => setOpen(open === value ? -1 : value);

    useEffect(() => {
        (async () => {
            setIsLoading(true);

            const result = await getNavigationCategories();
            setCategories(result);

            setIsLoading(false);
        })();
    }, []);

    if (isLoading) {
        return (
            <div>
                <Typography
                    as="div"
                    variant="paragraph"
                    className="mb-2 h-2 w-72 rounded-full bg-gray-300"
                >
                    &nbsp;
                </Typography>
                <Typography
                    as="div"
                    variant="paragraph"
                    className="mb-2 h-2 w-72 rounded-full bg-gray-300"
                >
                    &nbsp;
                </Typography>
                <Typography
                    as="div"
                    variant="paragraph"
                    className="mb-2 h-2 w-72 rounded-full bg-gray-300"
                >
                    &nbsp;
                </Typography>
            </div>
        );
    }

    return (
        <div className="flex flex-col gap-3">
            {categories.map((category, index) => (
                <Accordion
                    key={category.id}
                    open={open === index}
                    icon={
                        open === index ? (
                            <div>&#11205;</div>
                        ) : (
                            <div>&#11206;</div>
                        )
                    }
                >
                    <AccordionHeader
                        className="text-base py-1 border-none"
                        onClick={() => handleOpen(index)}
                    >
                        {category.name}
                    </AccordionHeader>
                    <AccordionBody>
                        <div className="flex flex-col gap-3">
                            {category.subCategories &&
                                category.subCategories.map((subCategory) => (
                                    <Link
                                        key={subCategory.id}
                                        onClick={onTapItem}
                                        href={`/category/${subCategory.slug}`}
                                        className="hover"
                                    >
                                        {subCategory.name}
                                    </Link>
                                ))}
                        </div>
                    </AccordionBody>
                </Accordion>
            ))}
        </div>
    );
}
