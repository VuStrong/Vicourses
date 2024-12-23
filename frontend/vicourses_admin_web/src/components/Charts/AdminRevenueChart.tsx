import { ApexOptions } from "apexcharts";
import { useEffect, useMemo, useState } from "react";
import ReactApexChart from "react-apexcharts";
import { AdminMetric } from "../../types/statistics";
import axiosInstance from "../../libs/axios";
import Loader from "../Loader";

const options: ApexOptions = {
    legend: {
        show: false,
        position: "top",
        horizontalAlign: "left",
    },
    colors: ["#3C50E0"],
    chart: {
        fontFamily: "Satoshi, sans-serif",
        height: 335,
        type: "area",
        dropShadow: {
            enabled: true,
            color: "#623CEA14",
            top: 10,
            blur: 4,
            left: 0,
            opacity: 0.1,
        },

        toolbar: {
            show: false,
        },
    },
    responsive: [
        {
            breakpoint: 1024,
            options: {
                chart: {
                    height: 300,
                },
            },
        },
        {
            breakpoint: 1366,
            options: {
                chart: {
                    height: 350,
                },
            },
        },
    ],
    stroke: {
        width: [2, 2],
        curve: "straight",
    },
    grid: {
        xaxis: {
            lines: {
                show: true,
            },
        },
        yaxis: {
            lines: {
                show: true,
            },
        },
    },
    dataLabels: {
        enabled: false,
    },
    markers: {
        size: 4,
        colors: "#fff",
        strokeColors: ["#3056D3", "#80CAEE"],
        strokeWidth: 3,
        strokeOpacity: 0.9,
        strokeDashArray: 0,
        fillOpacity: 1,
        discrete: [],
        hover: {
            size: undefined,
            sizeOffset: 5,
        },
    },
    xaxis: {
        type: "category",
        categories: [],
        axisBorder: {
            show: false,
        },
        axisTicks: {
            show: false,
        },
    },
    yaxis: {
        title: {
            style: {
                fontSize: "0px",
            },
        },
    },
};

const getRandomFakeData = () => {
    const data: AdminMetric[] = [];

    for (let index = 1; index <= 12; index++) {
        data.push({
            label: `${index}/2024`,
            revenue: Number((Math.random() * 9000 + 1000).toFixed(2)),
        })        
    }

    return data;
}

type Scope = "Week" | "Month" | "Year" | "All";

export default function AdminRevenueChart({
    initialMetrics,
}: {
    initialMetrics: AdminMetric[];
}) {
    const [state, setState] = useState<AdminMetric[]>(initialMetrics);
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [scope, setScope] = useState<Scope>("Month");

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<AdminMetric[]>("/api/stats/v1/admin/dashboard/scope-metrics", {
                params: { scope },
            })
            .then((response) => {
                setState(response.data);
                // setState(getRandomFakeData());
                setIsLoading(false);
            });
    }, [scope]);

    useEffect(() => {
        if (options.xaxis) options.xaxis.categories = state.map((d) => d.label);
    }, [state]);

    const label = useMemo(() => {
        const date = new Date();
        let from = new Date();

        if (scope === "Week") {
            from.setDate(from.getDate() - 7);
        } else if (scope === "Month") {
            from.setMonth(from.getMonth() - 1);
        } else if (scope === "Year") {
            from.setFullYear(from.getFullYear() - 1);
        } else {
            return "";
        }

        return `${from.toLocaleDateString()} - ${date.toLocaleDateString()}`;
    }, [scope]);

    return (
        <div className="col-span-12 rounded-sm border border-stroke bg-white px-5 pt-7.5 pb-5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:col-span-8">
            <div className="flex flex-wrap items-start justify-between gap-3 sm:flex-nowrap">
                <div className="flex w-full flex-wrap gap-3 sm:gap-5">
                    <div className="flex min-w-47.5">
                        <span className="mt-1 mr-2 flex h-4 w-full max-w-4 items-center justify-center rounded-full border border-primary">
                            <span className="block h-2.5 w-full max-w-2.5 rounded-full bg-primary"></span>
                        </span>
                        <div className="w-full">
                            <p className="font-semibold text-primary">
                                Total Revenue
                            </p>
                            <p className="text-sm font-medium">{label}</p>
                        </div>
                    </div>
                </div>
                <div className="flex w-full max-w-45 justify-end">
                    <div className="inline-flex items-center rounded-md bg-whiter p-1.5 dark:bg-meta-4">
                        <button
                            className={
                                scope === "Week"
                                    ? "rounded py-1 px-3 text-xs font-medium text-black hover:bg-white hover:shadow-card dark:text-white dark:hover:bg-boxdark"
                                    : "rounded bg-white py-1 px-3 text-xs font-medium text-black shadow-card hover:bg-white hover:shadow-card dark:bg-boxdark dark:text-white dark:hover:bg-boxdark"
                            }
                            onClick={() => setScope("Week")}
                            disabled={isLoading}
                        >
                            Week
                        </button>
                        <button
                            className={
                                scope === "Month"
                                    ? "rounded py-1 px-3 text-xs font-medium text-black hover:bg-white hover:shadow-card dark:text-white dark:hover:bg-boxdark"
                                    : "rounded bg-white py-1 px-3 text-xs font-medium text-black shadow-card hover:bg-white hover:shadow-card dark:bg-boxdark dark:text-white dark:hover:bg-boxdark"
                            }
                            onClick={() => setScope("Month")}
                            disabled={isLoading}
                        >
                            Month
                        </button>
                        <button
                            className={
                                scope === "Year"
                                    ? "rounded py-1 px-3 text-xs font-medium text-black hover:bg-white hover:shadow-card dark:text-white dark:hover:bg-boxdark"
                                    : "rounded bg-white py-1 px-3 text-xs font-medium text-black shadow-card hover:bg-white hover:shadow-card dark:bg-boxdark dark:text-white dark:hover:bg-boxdark"
                            }
                            onClick={() => setScope("Year")}
                            disabled={isLoading}
                        >
                            Year
                        </button>
                        <button
                            className={
                                scope === "All"
                                    ? "rounded py-1 px-3 text-xs font-medium text-black hover:bg-white hover:shadow-card dark:text-white dark:hover:bg-boxdark"
                                    : "rounded bg-white py-1 px-3 text-xs font-medium text-black shadow-card hover:bg-white hover:shadow-card dark:bg-boxdark dark:text-white dark:hover:bg-boxdark"
                            }
                            onClick={() => setScope("All")}
                            disabled={isLoading}
                        >
                            All
                        </button>
                    </div>
                </div>
            </div>

            <div>
                {isLoading ? (
                    <div className="flex justify-center items-center w-full h-[350px]">
                        <Loader />
                    </div>
                ) : (
                    <div id="chartOne" className="-ml-5">
                        <ReactApexChart
                            options={options}
                            series={[
                                {
                                    name: "Revenue",
                                    data: state.map((d) => d.revenue),
                                },
                            ]}
                            type="area"
                            height={350}
                        />
                    </div>
                )}
            </div>
        </div>
    );
}
