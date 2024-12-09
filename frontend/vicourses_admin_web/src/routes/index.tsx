import { Route, Routes } from 'react-router-dom';
import DefaultLayout from '../layout/DefaultLayout';
import PageTitle from '../components/PageTitle';
import ECommerce from '../pages/Dashboard/ECommerce';
import Calendar from '../pages/Calendar';
import Profile from '../pages/Profile';
import FormLayout from '../pages/Form/FormLayout';
import NotFound from '../pages/errors/NotFound';

const authRoutes = [
  {
    path: '/',
    element: (
      <>
        <PageTitle title="eCommerce Dashboard | TailAdmin - Tailwind CSS Admin Dashboard Template" />
        <ECommerce />
      </>
    ),
  },
  {
    path: '/calendar',
    element: (
      <>
        <PageTitle title="Calendar | TailAdmin - Tailwind CSS Admin Dashboard Template" />
        <Calendar />
      </>
    ),
  },
  {
    path: '/profile',
    element: (
      <>
        <PageTitle title="Profile | TailAdmin - Tailwind CSS Admin Dashboard Template" />
        <Profile />
      </>
    ),
  },
  {
    path: '/forms/form-layout',
    element: (
      <>
        <PageTitle title="Form Layout | TailAdmin - Tailwind CSS Admin Dashboard Template" />
        <FormLayout />
      </>
    ),
  },
  {
    path: '/',
    element: (
      <>
        <PageTitle title="eCommerce Dashboard | TailAdmin - Tailwind CSS Admin Dashboard Template" />
        <ECommerce />
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
