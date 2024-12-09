import { useEffect, useState } from 'react';
import Loader from './common/Loader';
import AppRoutes from './routes';
import { useLocation } from 'react-router-dom';

function App() {
  const [loading, setLoading] = useState<boolean>(true);
  const { pathname } = useLocation();

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  useEffect(() => {
    setTimeout(() => setLoading(false), 1000);
  }, []);

  return loading ? <Loader /> : <AppRoutes />;
}

export default App;
