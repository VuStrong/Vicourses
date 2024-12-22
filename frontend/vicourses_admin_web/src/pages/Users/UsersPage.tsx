import { useEffect, useState } from "react";
import { useLocation, useNavigate, useSearchParams } from "react-router-dom";

import { User } from "../../types/user";

export default function UsersPage() {
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [users, setUsers] = useState<User[]>([]);
    const location = useLocation();
    const navigate = useNavigate();
    const queryParams = new URLSearchParams(location.search);
    const currentPage = Number(queryParams.get("page")) || 1;

    const [searchParams, setSearchParams] = useSearchParams();

    useEffect(() => {
        //
    }, []);

    return (
        <>
            <div className="flex items-center justify-between flex-wrap">
                <h1 className="text-2xl font-bold mb-5">Users</h1>
            </div>
            <div>
                
            </div>
        </>
    );
}
