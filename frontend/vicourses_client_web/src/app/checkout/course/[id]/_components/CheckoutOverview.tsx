"use client";

import { DEFAULT_COURSE_THUMBNAIL_URL } from "@/libs/constants";
import { Course } from "@/libs/types/course";

export default function CheckoutOverview({ course }: { course: Course }) {
    return (
        <div className="w-full">
            <h2 className="text-black font-bold text-xl mb-7">Overview</h2>

            <h4 className="text-gray-800 font-bold text-lg mb-3">
                Payment method
            </h4>
            <div className="border border-gray-300 mb-5">
                <div className="flex gap-2 border-b border-gray-300 bg-[#f7f9fa] px-3">
                    <div className="w-24">
                        <img
                            src="/img/paypal-logo.png"
                            alt="Paypal"
                            className="w-full h-full"
                        />
                    </div>
                </div>
                <div className="bg-white p-3 text-gray-800">
                    To complete the transaction, we'll transfer you to PayPal's
                    secure servers.
                </div>
            </div>

            <h4 className="text-gray-800 font-bold text-lg mb-3">Info</h4>
            <div className="flex gap-2">
                <div className="aspect-square w-16 h-16">
                    <img
                        src={
                            course.thumbnailUrl || DEFAULT_COURSE_THUMBNAIL_URL
                        }
                        alt={course.title}
                        className="w-full h-full object-cover"
                    />
                </div>
                <div className="text-gray-800 flex-1">
                    <div className="font-bold line-clamp-2">
                        {course.title}
                    </div>
                    <div>${course.price}</div>
                </div>
            </div>
        </div>
    );
}
