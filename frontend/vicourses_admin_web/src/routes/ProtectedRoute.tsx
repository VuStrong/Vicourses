import React, { useEffect } from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import useAccount from '../hooks/useAccount';

const ProtectedRoute = () => {
    const status = useAccount(state => state.status);
    const initialize = useAccount(state => state.initialize);

    useEffect(() => {
        initialize();
    }, []);

    if (status === "unauthenticated") { 
        return <Navigate to="/auth/login" replace />;
    }
    return <Outlet />;
};

export default ProtectedRoute;