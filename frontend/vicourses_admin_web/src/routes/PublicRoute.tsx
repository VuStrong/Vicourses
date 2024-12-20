import { Navigate, Outlet } from 'react-router-dom';
import useAccount from '../hooks/useAccount';
import { useEffect } from 'react';

const PublicRoute = () => {
  const initialize = useAccount(state => state.initialize);
  const status = useAccount(state => state.status);

  useEffect(() => {
    console.log(111);
    
    initialize();
  }, []);

  if (status === "authenticated") {
    return <Navigate to="/dashboard" replace />;
  }
  return <Outlet />;
};

export default PublicRoute;
