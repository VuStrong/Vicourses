"use client";

import { Loader } from "@/components/common";
import { InstructorPerformance, Scope } from "@/libs/types/statistics";
import { useSession } from "next-auth/react";
import { useEffect, useMemo, useState } from "react";
import { Line } from "react-chartjs-2";
import "chart.js/auto";
import { Select, Option } from "@material-tailwind/react";
import { getInstructorPerformance } from "@/services/api/statistics";
import { getInstructorCourses } from "@/services/api/course";
import AsyncSelect from "react-select/async";
import { performanceFake } from "./fakeData";

type CourseOption = {
    value: string;
    label: string;
}

export default function PerformanceCharts() {
    const [performance, setPerformance] =
        useState<InstructorPerformance | null>(null);
    const [scope, setScope] = useState<Scope>("Week");
    const [course, setCourse] = useState<CourseOption>();
    const { data: session, status } = useSession();

    const labels = useMemo(() => {
        return performance?.metrics.map((m) => m.label) || [];
    }, [performance]);

    useEffect(() => {
        if (status === "authenticated") {
            (async () => {
                setPerformance(null);

                const result = await getInstructorPerformance(
                    {
                        scope,
                        courseId: course?.value,
                    },
                    session.accessToken
                );

                setPerformance(result);
                // setPerformance(performanceFake);
            })();
        }
    }, [status, scope, course]);

    return (
        <div>
            <div className="mb-5 flex flex-col gap-3">
                <div className="max-w-[200px]">
                    <Select
                        disabled={!performance}
                        label="Scope"
                        value={scope}
                        onChange={(value) => setScope(value as Scope)}
                    >
                        <Option value="Week">7 days ago</Option>
                        <Option value="Month">30 days ago</Option>
                        <Option value="Year">12 months ago</Option>
                        <Option value="All">All time</Option>
                    </Select>
                </div>
                <div>
                    <CourseSelect
                        course={course}
                        onCourseChange={(c) => setCourse(c)}
                        disabled={!performance}
                        accessToken={session?.accessToken || ""}
                        instructorId={session?.user.id || ""}
                    />
                </div>
            </div>

            {!performance ? (
                <div className="flex justify-center">
                    <Loader />
                </div>
            ) : (
                <div className="flex flex-col gap-10">
                    <div className="border border-gray-800 shadow-xl p-10 overflow-x-auto w-[80vw] md:w-full">
                        <div className="text-gray-800 text-lg">
                            Total revenue
                        </div>
                        <div className="text-black text-2xl">
                            ${performance.totalRevenue}
                        </div>

                        {performance.metrics[0] ? (
                            <Chart
                                labels={labels}
                                dataLabel="Revenue"
                                data={performance.metrics.map((m) => m.revenue)}
                            />
                        ) : (
                            <div className="text-black flex justify-center items-center h-52">
                                No data to display
                            </div>
                        )}
                    </div>
                    <div className="border border-gray-800 shadow-xl p-10 overflow-x-auto w-[80vw] md:w-full">
                        <div className="text-gray-800 text-lg">
                            Total enrollment
                        </div>
                        <div className="text-black text-2xl">
                            {performance.totalEnrollmentCount}
                        </div>

                        {performance.metrics[0] ? (
                            <Chart
                                labels={labels}
                                dataLabel="Enrollments"
                                data={performance.metrics.map(
                                    (m) => m.enrollmentCount
                                )}
                            />
                        ) : (
                            <div className="text-black flex justify-center items-center h-52">
                                No data to display
                            </div>
                        )}
                    </div>
                    <div className="border border-gray-800 shadow-xl p-10 overflow-x-auto w-[80vw] md:w-full">
                        <div className="text-gray-800 text-lg">
                            Total refund
                        </div>
                        <div className="text-black text-2xl">
                            {performance.totalRefundCount}
                        </div>

                        {performance.metrics[0] ? (
                            <Chart
                                labels={labels}
                                dataLabel="Refunds"
                                data={performance.metrics.map(
                                    (m) => m.refundCount
                                )}
                            />
                        ) : (
                            <div className="text-black flex justify-center items-center h-52">
                                No data to display
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
}

function Chart({
    labels,
    dataLabel,
    data,
}: {
    labels: string[];
    dataLabel: string;
    data: any[];
}) {
    return (
        <div className="w-[600px] lg:w-full h-[50vh] relative">
            <Line
                style={{
                    width: "100%",
                    height: "100%",
                }}
                options={{
                    maintainAspectRatio: false,
                }}
                data={{
                    labels: labels,
                    datasets: [
                        {
                            label: dataLabel,
                            data: data,
                        },
                    ],
                }}
            />
        </div>
    );
}

const loadCourses = async (
    inputValue: string,
    instructorId: string,
    accessToken: string
) => {
    const result = await getInstructorCourses(
        {
            instructorId,
            limit: 50,
            q: inputValue || undefined,
        },
        accessToken
    );

    if (!result) return [];

    return result.items.map((c) => ({
        value: c.id,
        label: c.title,
    }));
};

function CourseSelect({
    course,
    onCourseChange,
    accessToken,
    instructorId,
    disabled,
}: {
    course?: CourseOption;
    onCourseChange: (course?: CourseOption) => void;
    accessToken: string;
    instructorId: string;
    disabled: boolean;
}) {
    return (
        <AsyncSelect
            instanceId="courses"
            placeholder="Select course"
            cacheOptions
            isClearable
            loadOptions={async (value) =>
                await loadCourses(value, instructorId, accessToken)
            }
            isDisabled={disabled}
            value={course}
            onChange={(data) => onCourseChange(data || undefined)}
        />
    );
}
