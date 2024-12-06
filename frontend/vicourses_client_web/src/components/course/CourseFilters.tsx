import { CourseLevel } from "@/libs/types/course";
import CourseFilterCollapse from "./CourseFilterCollapse";
import { Checkbox, Rating, Typography } from "@material-tailwind/react";

export type CourseFilterOptions = {
    free?: boolean;
    level?: CourseLevel;
    rating?: number;
};

export default function CourseFilters({
    filter,
    onFilterChange,
}: {
    filter: CourseFilterOptions;
    onFilterChange?: (filter: CourseFilterOptions) => void;
}) {
    const onRatingChange = (rating?: number) => {
        onFilterChange?.({
            ...filter,
            rating,
        });
    };

    const onFreeChange = (free?: boolean) => {
        onFilterChange?.({
            ...filter,
            free,
        });
    };

    const onLevelChange = (level?: CourseLevel) => {
        onFilterChange?.({
            ...filter,
            level,
        });
    };

    return (
        <div>
            <CourseFilterCollapse filterName="Rating">
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={filter.rating === 4}
                        onChange={(e) =>
                            onRatingChange(e.target.checked ? 4 : undefined)
                        }
                        crossOrigin={undefined}
                    />
                    <Rating value={4} readonly />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        From 4 and above
                    </Typography>
                </div>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={filter.rating === 3}
                        onChange={(e) =>
                            onRatingChange(e.target.checked ? 3 : undefined)
                        }
                        crossOrigin={undefined}
                    />
                    <Rating value={3} readonly />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        From 3 and above
                    </Typography>
                </div>
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={filter.rating === 2}
                        onChange={(e) =>
                            onRatingChange(e.target.checked ? 2 : undefined)
                        }
                        crossOrigin={undefined}
                    />
                    <Rating value={2} readonly />
                    <Typography
                        color="blue-gray"
                        className="font-medium text-blue-gray-500"
                    >
                        From 2 and above
                    </Typography>
                </div>
            </CourseFilterCollapse>
            <CourseFilterCollapse filterName="Price">
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={filter.free}
                        onChange={(e) =>
                            onFreeChange(e.target.checked ? true : undefined)
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
                        checked={filter.free === false}
                        onChange={(e) =>
                            onFreeChange(e.target.checked ? false : undefined)
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
            </CourseFilterCollapse>
            <CourseFilterCollapse filterName="Level">
                <div className="flex items-center gap-2 text-blue-gray-500">
                    <Checkbox
                        checked={filter.level === "All"}
                        onChange={(e) =>
                            onLevelChange(e.target.checked ? "All" : undefined)
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
                        checked={filter.level === "Basic"}
                        onChange={(e) =>
                            onLevelChange(e.target.checked ? "Basic" : undefined)
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
                        checked={filter.level === "Intermediate"}
                        onChange={(e) =>
                            onLevelChange(e.target.checked ? "Intermediate" : undefined)
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
                        checked={filter.level === "Expert"}
                        onChange={(e) =>
                            onLevelChange(e.target.checked ? "Expert" : undefined)
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
            </CourseFilterCollapse>
        </div>
    );
}
