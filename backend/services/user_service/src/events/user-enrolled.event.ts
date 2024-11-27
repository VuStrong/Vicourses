import { usersRepository } from "../data/repositories";
import { AppError } from "../utils/app-error";

type UserEnrolledEvent = {
    userId: string;
    course: {
        id: string;
        user: {
            id: string;
        },
        subCategory: {
            id: string;
        },
        tags: string[],
    }
}

export default async function handleUserEnrolledEvent(event: UserEnrolledEvent) {
    if (event.course?.user?.id) await updateInstructorEnrollmentCount(event.course.user.id);

    if (event.userId) await addCourseTagToUser(event);
}

async function updateInstructorEnrollmentCount(id: string) {
    await usersRepository.createQueryBuilder()
        .update()
        .where("id = :id", { id })
        .set({ totalEnrollmentCount: () => "totalEnrollmentCount + 1" })
        .execute();
}

async function addCourseTagToUser(event: UserEnrolledEvent) {
    const user = await usersRepository.findOne({
        where: { id: event.userId },
        select: {
            id: true,
            courseTags: true,
            categoryIds: true,
        }
    });

    if (!user) {
        throw new AppError("User not found", 404);
    }

    const tag = event.course?.tags?.[0];
    const categoryId = event.course?.subCategory?.id;
    
    let userCourseTags = user.courseTags.split(",");
    let userCategories = user.categoryIds.split(",");

    if (tag && !userCourseTags.includes(tag)) {
        userCourseTags.unshift(tag);
    }
    if (categoryId && !userCategories.includes(categoryId)) {
        userCategories.unshift(categoryId);
    }

    await usersRepository.update(user.id, {
        courseTags: userCourseTags.join(","),
        categoryIds: userCategories.join(","),
    });
}