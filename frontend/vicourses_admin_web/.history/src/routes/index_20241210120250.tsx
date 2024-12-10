import { Route, Routes } from 'react-router-dom';
import DefaultLayout from '../layout/DefaultLayout';
import PageTitle from '../components/PageTitle';
import NotFound from '../pages/errors/NotFound';
import Login from '../pages/Authentication/SignIn';

const authRoutes = [
  {
    path: '/auth/login',
    element: <Login />,
  }
];
const userRoutes = [
  {
    path: '/',
    element: <Home />,
  },
  {
    path: '/search',
    element: <Find />,
  },
];

const accountRoutes = [
  {
    path: '/account/',
    element: <ChangeAccountForm/>,
  },
  {
    path: '/account/password',
    element:<ChangePassForm/>,
  },
];
const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/auth">
        {authRoutes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Route>
      <Route path="/" element={<DefaultLayout/>}>
        {authRoutes.map((route, index) => (
          <Route key={index} path={route.path} element={route.element} />
        ))}
      </Route>
      <Route path="*" element={<NotFound />} />
    </Routes>
  );
};

export default AppRoutes;
