import { Rating } from "@/libs/types/rating";

export const ratingsFake: Rating[] = [
    {
        id: "ratingid1",
        courseId: "course1",
        course: null,
        userId: "user1",
        user: {
            id: "user1",
            name: "Vu Manh",
            thumbnailUrl: "http://res.cloudinary.com/dsrcm9jcs/image/upload/v1707822481/User%20images/lvawm7oxoxut850g6nit.jpg",
        },
        feedback: "Very good course",
        star: 4,
        createdAt: "2024-12-12T11:20:10.541Z",
        responded: true,
        response: "Thank you for your rating :>",
        respondedAt: "2024-12-12T11:20:10.541Z"
    },
    {
        id: "ratingid2",
        courseId: "course1",
        course: null,
        userId: "user1",
        user: {
            id: "user2",
            name: "Vu Manh 22",
            thumbnailUrl: "http://res.cloudinary.com/dsrcm9jcs/image/upload/v1707822481/User%20images/lvawm7oxoxut850g6nit.jpg",
        },
        feedback: "Very bad course",
        star: 1,
        createdAt: "2024-12-12T11:20:10.541Z",
        responded: false,
        response: null,
        respondedAt: null,
    },
];