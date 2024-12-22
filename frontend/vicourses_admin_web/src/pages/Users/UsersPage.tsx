import { useEffect, useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";

import { User } from "../../types/user";
import { PagedResult } from "../../types/common";
import axiosInstance from "../../libs/axios";
import { DEFAULT_USER_AVATAR_URL } from "../../libs/contants";
import Loader from "../../components/Loader";
import { Select, Option } from "../../components/Forms";

const pageSize = 10;
const selectFields = "id,name,email,role,createdAt,thumbnailUrl";

export default function UsersPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [users, setUsers] = useState<User[]>([]);
    const [total, setTotal] = useState<number>(0);

    const location = useLocation();
    const navigate = useNavigate();
    const queryParams = new URLSearchParams(location.search);

    const currentPage = Number(queryParams.get("page")) || 1;
    const totalPage = Math.ceil(total / pageSize);
    const role = queryParams.get("role");

    useEffect(() => {
        setIsLoading(true);

        axiosInstance
            .get<PagedResult<User>>(`/api/us/v1/users`, {
                params: {
                    skip: (currentPage - 1) * pageSize,
                    limit: pageSize,
                    order: "createdAt_desc",
                    fields: selectFields,
                    role,
                },
            })
            .then((response) => {
                setUsers(response.data.items);
                setTotal(response.data.total);
                setIsLoading(false);
            });
    }, [currentPage, role]);

    const handleChangePage = (page: number) => {
        if (page === currentPage) return;

        queryParams.set("page", `${page}`);
        navigate({ search: queryParams.toString() });
    };

    const handleChangeRole = (roleToChange: string) => {
        if (roleToChange === role) return;

        if (roleToChange) {
            queryParams.set("role", roleToChange);
        } else {
            queryParams.delete("role");
        }
        queryParams.delete("page");
        navigate({ search: queryParams.toString() });
    };

    return (
        <>
            <div className="flex items-center justify-between flex-wrap">
                <h1 className="text-2xl font-bold mb-5">Users ({total})</h1>
            </div>
            <div className="flex gap-3 flex-wrap mb-5">
                <Select
                    value={role || ""}
                    onChange={(e) => handleChangeRole(e.target.value)}
                    disabled={isLoading}
                >
                    <Option value="">Select role</Option>
                    <Option value="admin">Admin</Option>
                    <Option value="instructor">Instructor</Option>
                    <Option value="student">Student</Option>
                </Select>
            </div>
            <div>
                {isLoading && (
                    <div className="flex justify-center">
                        <Loader />
                    </div>
                )}

                {!isLoading &&
                    (users.length === 0 ? (
                        <div className="text-center">No user found</div>
                    ) : (
                        <>
                            <UsersTable users={users} />
                            <div className="flex mt-5">
                                <button
                                    disabled={currentPage <= 1}
                                    onClick={() =>
                                        handleChangePage(currentPage - 1)
                                    }
                                    className="flex items-center justify-center px-3 h-8 ms-0 leading-tight text-gray-500 bg-white border border-e-0 border-gray-300 rounded-s-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 disabled:opacity-50"
                                >
                                    Previous
                                </button>
                                <button
                                    disabled={currentPage >= totalPage}
                                    onClick={() =>
                                        handleChangePage(currentPage + 1)
                                    }
                                    className="flex items-center justify-center px-3 h-8 leading-tight text-gray-500 bg-white border border-gray-300 rounded-e-lg hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 disabled:opacity-50"
                                >
                                    Next
                                </button>
                            </div>
                        </>
                    ))}
            </div>
        </>
    );
}

function UsersTable({ users }: { users: User[] }) {
    return (
        <div className="rounded-sm border border-stroke bg-white pt-2 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark xl:pb-1">
            <div className="max-w-full overflow-x-auto">
                <table className="w-full min-w-max table-auto text-left">
                    <thead>
                        <tr>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Name
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Email
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Role
                            </th>
                            <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white">
                                Created At
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user, index) => {
                            return (
                                <tr key={index}>
                                    <td className="p-4">
                                        <div className="flex items-center gap-3">
                                            <div className="h-10 w-10">
                                                <img
                                                    src={
                                                        user.thumbnailUrl ||
                                                        DEFAULT_USER_AVATAR_URL
                                                    }
                                                    alt={user.name}
                                                    className="w-full h-full object-cover rounded-full"
                                                />
                                            </div>
                                            <Link
                                                to={`/users/${user.id}`}
                                                className="hover:text-primary"
                                            >
                                                {user.name}
                                            </Link>
                                        </div>
                                    </td>
                                    <td className="p-4">{user.email}</td>
                                    <td className="p-4">{user.role}</td>
                                    <td className="p-4">
                                        {new Date(
                                            user.createdAt,
                                        ).toLocaleDateString()}
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
}
