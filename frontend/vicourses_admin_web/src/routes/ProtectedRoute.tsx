import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

const ProtectedRoute = () => {
    const account = localStorage.getItem('account');

    if (!account) { 
        return <Navigate to="/auth/login" replace />;
    }
    return <Outlet />;
};

export default ProtectedRoute;