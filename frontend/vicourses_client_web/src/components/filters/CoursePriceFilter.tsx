"use client";

import {
    Accordion,
    AccordionBody,
    AccordionHeader,
    Checkbox,
    Typography,
} from "@material-tailwind/react";
import { useState } from "react";

export default function CoursePriceFilter({
    free,
    onFreeChange,
    disabled,
}: {
    free?: boolean;
    onFreeChange?: (free?: boolean) => void;
    disabled?: boolean;
}) {
    const [isOpen, setIsOpen] = useState<boolean>(false);

    return (
        <Accordion
            disabled={disabled}
            open={isOpen}
            icon={isOpen ? <div>&#11205;</div> : <div>&#11206;</div>}
        >
            <AccordionHeader onClick={() => setIsOpen(!isOpen)}>
                Price
            </AccordionHeader>
            <AccordionBody>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={free}
                        onChange={(e) =>
                            onFreeChange?.(e.target.checked ? true : undefined)
                        }
                        crossOrigin={undefined}
                    />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        Free
                    </Typography>
                </div>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={free === false}
                        onChange={(e) =>
                            onFreeChange?.(e.target.checked ? false : undefined)
                        }
                        crossOrigin={undefined}
                    />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        Paid
                    </Typography>
                </div>
            </AccordionBody>
        </Accordion>
    );
}
