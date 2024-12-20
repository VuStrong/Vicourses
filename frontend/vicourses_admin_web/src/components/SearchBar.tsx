import { useState } from "react";
import { IoMdSearch } from "react-icons/io";

export default function SearchBar({
    placeholder,
    onSubmit,
}: {
    placeholder?: string,
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
                placeholder={placeholder}
                value={value}
                onChange={(e) => setValue(e.target.value)}
                className="flex-grow pl-10 border-none outline-none bg-transparent"
            />

            <button
                className="absolute left-5 top-1/2 -translate-y-1/2"
                type="submit"
            >
                <IoMdSearch size={24} />
            </button>
        </form>
    );
}