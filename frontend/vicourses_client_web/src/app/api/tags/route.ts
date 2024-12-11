import fs from "fs";
import { NextResponse } from "next/server";

export async function GET(request: Request) {
    const file = fs.readFileSync("public/data/tags.json");
    var tags = JSON.parse(file.toString()) as string[];

    const url = new URL(request.url);
    const searchParams = new URLSearchParams(url.searchParams);
    const q = searchParams.get("q")?.trim().toLowerCase();

    if (q) {
        tags = tags.filter(tag => tag.includes(q));
    }

    return NextResponse.json({ tags });
}
