"use client";

import { useState } from "react";
import { IoMdSearch } from "react-icons/io";

export default function SearchBar({
    onSubmit,
}: {
    onSubmit: (value: string) => void;
}) {
    const [value, setValue] = useState<string>("");

    return (
        <form
            onSubmit={(e) => {
                e.preventDefault();
                onSubmit(value);
            }}
            className="relative flex border border-gray-600 rounded-full p-3"
        >
            <input
                type="search"
                placeholder="Search for courses"
                value={value}
                onChange={(e) => setValue(e.target.value)}
                className="flex-grow pl-10 border-none outline-none"
            />

            <button
                className="absolute left-5 top-1/2 -translate-y-1/2"
                type="submit"
                disabled={!value}
            >
                <IoMdSearch size={24} />
            </button>
        </form>
    );
}
