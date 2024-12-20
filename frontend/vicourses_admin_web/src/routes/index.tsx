import { Route, Routes } from "react-router-dom";

import DefaultLayout from "../layout/DefaultLayout";
import ProtectedRoute from "./ProtectedRoute";
import PublicRoute from "./PublicRoute";

import NotFound from "../pages/errors/NotFound";
import LoginPage from "../pages/authentication/LoginPage";
import DashboardPage from "../pages/DashboardPage";
import CategoriesPage from "../pages/categories/CategoriesPage";
import CategoryEditPage from "../pages/categories/CategoryEditPage";

const AppRoutes = () => {
    return (
        <Routes>
            <Route path="/auth" element={<PublicRoute />}>
                {/* Login page */}
                <Route path="login" element={<LoginPage />} />
            </Route>

            <Route path="/" element={<ProtectedRoute />}>
                <Route element={<DefaultLayout />}>
                    {/* Dashboard page */}
                    <Route index path="" element={<DashboardPage />} />

                    {/* Category pages */}
                    <Route path="categories">
                        <Route path="" element={<CategoriesPage />} />
                        <Route path=":slug" element={<CategoryEditPage />} />
                    </Route>
                </Route>
            </Route>

            <Route path="*" element={<NotFound />} />
        </Routes>
    );
};

export default AppRoutes;
