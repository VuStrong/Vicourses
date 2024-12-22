import { Navigate, Outlet } from "react-router-dom";
import useUser from "../hooks/useUser";

const PublicRoute = () => {
    const status = useUser((state) => state.status);

    if (status === "authenticated") {
        return <Navigate to="/" replace />;
    }
    
    return <Outlet />;
};

export default PublicRoute;
