"use client";

import { CourseLevel } from "@/libs/types/course";
import {
    Accordion,
    AccordionBody,
    AccordionHeader,
    Checkbox,
    Typography,
} from "@material-tailwind/react";
import { useState } from "react";

export default function CourseLevelFilter({
    level,
    onLevelChange,
    disabled,
}: {
    level?: CourseLevel;
    onLevelChange?: (level?: CourseLevel) => void;
    disabled?: boolean;
}) {
    const [isOpen, setIsOpen] = useState<boolean>(true);

    return (
        <Accordion
            disabled={disabled}
            open={isOpen}
            icon={isOpen ? <div>&#11205;</div> : <div>&#11206;</div>}
        >
            <AccordionHeader onClick={() => setIsOpen(!isOpen)}>
                Level
            </AccordionHeader>
            <AccordionBody>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={level === "All"}
                        onChange={(e) =>
                            onLevelChange?.(
                                e.target.checked ? "All" : undefined
                            )
                        }
                        crossOrigin={undefined}
                    />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        All
                    </Typography>
                </div>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={level === "Basic"}
                        onChange={(e) =>
                            onLevelChange?.(
                                e.target.checked ? "Basic" : undefined
                            )
                        }
                        crossOrigin={undefined}
                    />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        Basic
                    </Typography>
                </div>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={level === "Intermediate"}
                        onChange={(e) =>
                            onLevelChange?.(
                                e.target.checked ? "Intermediate" : undefined
                            )
                        }
                        crossOrigin={undefined}
                    />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        Intermediate
                    </Typography>
                </div>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={level === "Expert"}
                        onChange={(e) =>
                            onLevelChange?.(
                                e.target.checked ? "Expert" : undefined
                            )
                        }
                        crossOrigin={undefined}
                    />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        Expert
                    </Typography>
                </div>
            </AccordionBody>
        </Accordion>
    );
}
