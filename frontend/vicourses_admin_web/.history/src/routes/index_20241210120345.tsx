import { Route, Routes } from 'react-router-dom';
import DefaultLayout from '../layout/DefaultLayout';
import PageTitle from '../components/PageTitle';
import NotFound from '../pages/errors/NotFound';
import Login from '../pages/Authentication/SignIn';

const authRoutes = [
  {
    path: '/auth/login',
    element: <Login />,
  },
];
const userRoutes = [
  {
    path: '/',
    element: <></>,
  }
];
const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/auth">
        {authRoutes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Route>
      <Route path="/" element={<DefaultLayout />}>
        {userRoutes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Route>
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes;
