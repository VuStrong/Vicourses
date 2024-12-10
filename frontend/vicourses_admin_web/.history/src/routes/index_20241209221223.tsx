import { Route, Routes } from 'react-router-dom';
import DefaultLayout from '../layout/DefaultLayout';
import PageTitle from '../components/PageTitle';
import NotFound from '../pages/errors/NotFound';
import Login from '../pages/Authentication/SignIn';

const authRoutes = [
  {
    path: '/',
    element: (
      <>
        <PageTitle title="eCommerce Dashboard | TailAdmin - Tailwind CSS Admin Dashboard Template" />
        <Login/>
      </>
    ),
  },
  {
    path: '/calendar',
    element: (
      <>
        <PageTitle title="Calendar | TailAdmin - Tailwind CSS Admin Dashboard Template" />
      </>
    ),
  },
  {
    path: '/profile',
    element: (
      <>
        <PageTitle title="Profile | TailAdmin - Tailwind CSS Admin Dashboard Template" />
      </>
    ),
  },
  {
    path: '/forms/form-layout',
    element: (
      <>
        <PageTitle title="Form Layout | TailAdmin - Tailwind CSS Admin Dashboard Template" />
      </>
    ),
  },
  {
    path: '/',
    element: (
      <>
        <PageTitle title="eCommerce Dashboard | TailAdmin - Tailwind CSS Admin Dashboard Template" />
      </>
    ),
  },
];
const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<DefaultLayout />}>
        {authRoutes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Route>
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes;
