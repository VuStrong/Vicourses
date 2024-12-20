import { Navigate, Outlet } from "react-router-dom";
import useUser from "../hooks/useUser";

const ProtectedRoute = () => {
    const status = useUser((state) => state.status);

    if (status === "unauthenticated") {
        return <Navigate to="/auth/login" replace />;
    }
    
    return <Outlet />;
};

export default ProtectedRoute;
