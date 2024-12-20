import { useEffect } from 'react';
import AppRoutes from './routes';
import { useLocation } from 'react-router-dom';

function App() {
  const { pathname } = useLocation();

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  return <AppRoutes />;
}

export default App;
