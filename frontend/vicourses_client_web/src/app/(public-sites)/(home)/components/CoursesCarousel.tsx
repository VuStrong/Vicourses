"use client";

import Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";

import { Course } from "@/libs/types/course";
import CourseCard from "@/components/course/CourseCard";

const responsive = {
    desktop: {
        breakpoint: { max: 3000, min: 1024 },
        items: 5,
    },
    tablet: {
        breakpoint: { max: 1024, min: 768 },
        items: 3,
    },
    mobile: {
        breakpoint: { max: 768, min: 0 },
        items: 1,
    },
};

export default function CoursesCarousel({ courses }: { courses: Course[] }) {
    return (
        <Carousel ssr={true} itemClass="px-1" containerClass="-mx-1" responsive={responsive}>
            {courses.map((course) => (
                <CourseCard key={course.id} course={course} />
            ))}
        </Carousel>
    );
}
