"use client";

import {
    Accordion,
    AccordionBody,
    AccordionHeader,
    Checkbox,
    Rating,
    Typography,
} from "@material-tailwind/react";
import { useState } from "react";

export default function CourseRatingFilter({
    rating,
    onRatingChange,
    disabled,
}: {
    rating?: number;
    onRatingChange?: (rating?: number) => void;
    disabled?: boolean
}) {
    const [isOpen, setIsOpen] = useState<boolean>(true);

    return (
        <Accordion
            disabled={disabled}
            open={isOpen}
            icon={isOpen ? <div>&#11205;</div> : <div>&#11206;</div>}
        >
            <AccordionHeader onClick={() => setIsOpen(!isOpen)}>
                Rating
            </AccordionHeader>
            <AccordionBody>
                <div className="flex flex-col gap-5">
                    <div className="flex items-center gap-2 text-blue-gray-500">
                        <Checkbox
                            checked={rating === 4}
                            onChange={(e) =>
                                onRatingChange?.(
                                    e.target.checked ? 4 : undefined
                                )
                            }
                            crossOrigin={undefined}
                        />
                        <div>
                            <Rating value={4} readonly />
                            <Typography
                                color="blue-gray"
                                className="font-medium text-blue-gray-500"
                            >
                                From 4 and above
                            </Typography>
                        </div>
                    </div>
                    <div className="flex items-center gap-2 text-blue-gray-500">
                        <Checkbox
                            checked={rating === 3}
                            onChange={(e) =>
                                onRatingChange?.(
                                    e.target.checked ? 3 : undefined
                                )
                            }
                            crossOrigin={undefined}
                        />
                        <div>
                            <Rating value={3} readonly />
                            <Typography
                                color="blue-gray"
                                className="font-medium text-blue-gray-500"
                            >
                                From 3 and above
                            </Typography>
                        </div>
                    </div>
                    <div className="flex items-center gap-2 text-blue-gray-500">
                        <Checkbox
                            checked={rating === 2}
                            onChange={(e) =>
                                onRatingChange?.(
                                    e.target.checked ? 2 : undefined
                                )
                            }
                            crossOrigin={undefined}
                        />
                        <div>
                            <Rating value={2} readonly />
                            <Typography
                                color="blue-gray"
                                className="font-medium text-blue-gray-500"
                            >
                                From 2 and above
                            </Typography>
                        </div>
                    </div>
                </div>
            </AccordionBody>
        </Accordion>
    );
}
