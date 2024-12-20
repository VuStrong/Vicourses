import { Route, Routes } from 'react-router-dom';
import DefaultLayout from '../layout/DefaultLayout';
import PageTitle from '../components/PageTitle';
import NotFound from '../pages/errors/NotFound';
import Login from '../pages/Authentication/SignIn';
import ProtectedRoute from './ProtectedRoute';
import TableOne from '../components/Tables/TableOne';
import PublicRoute from './PublicRoute';

const authRoutes = [
  {
    path: 'login',
    element: (
      <>
        <PageTitle title="Login" />
        <Login />
      </>
    ),
  },
];
const userRoutes = [
  {
    index: true,
    path: 'dashboard',
    element: (
      <>
        <PageTitle title="Dashboard" />
        <TableOne />
      </>
    ),
  },
];
const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/auth" element={<PublicRoute />}>
        {authRoutes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Route>
      <Route path='/' element={<ProtectedRoute />}>
        <Route element={<DefaultLayout />}>
          {userRoutes.map((route, index) => (
            <Route key={index} path={route.path} element={route.element} index={route.index} />
          ))}
        </Route>
      </Route>
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes;
