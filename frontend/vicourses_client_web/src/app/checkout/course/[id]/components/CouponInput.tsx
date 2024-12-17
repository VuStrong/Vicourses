"use client";

import { useState } from "react";
import { Button, Input } from "@material-tailwind/react";

export default function CouponInput({
    onApply,
}: {
    onApply: (value: string) => void,
}) {
    const [value, setValue] = useState<string>("");

    return (
        <div className="relative flex w-full max-w-[24rem]">
            <Input
                crossOrigin={undefined}
                label="Enter coupon"
                value={value}
                onChange={(e) => setValue(e.target.value)}
                className="pr-20 rounded-none"
                containerProps={{
                    className: "min-w-0",
                }}
            />
            <Button
                size="sm"
                color={value ? "gray" : "blue-gray"}
                disabled={!value}
                type="button"
                className="!absolute right-1 top-1 rounded-none"
                onClick={() => onApply(value)}
            >
                Apply
            </Button>
        </div>
    );
}
