import { Route, Routes } from "react-router-dom";

import DefaultLayout from "../layout/DefaultLayout";
import ProtectedRoute from "./ProtectedRoute";
import PublicRoute from "./PublicRoute";

import NotFound from "../pages/Errors/NotFound";
import LoginPage from "../pages/Authentication/LoginPage";
import DashboardPage from "../pages/DashboardPage";
import CategoriesPage from "../pages/Categories/CategoriesPage";
import CategoryEditPage from "../pages/Categories/CategoryEditPage";
import CategoryCreatePage from "../pages/Categories/CategoryCreatePage";
import UsersPage from "../pages/Users/UsersPage";
import UserDetailPage from "../pages/Users/UserDetailPage";
import CoursesPage from "../pages/Courses/CoursesPage";
import CourseDetailPage from "../pages/Courses/CourseDetailPage";
import PaymentsPage from "../pages/Payments/PaymentsPage";
import PaymentDetailPage from "../pages/Payments/PaymentDetailPage";

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
                        <Route path="create" element={<CategoryCreatePage />} />
                    </Route>

                    {/* Course pages */}
                    <Route path="courses">
                        <Route path="" element={<CoursesPage />} />
                        <Route path=":id" element={<CourseDetailPage />} />
                    </Route>

                    {/* User pages */}
                    <Route path="users">
                        <Route path="" element={<UsersPage />} />
                        <Route path=":id" element={<UserDetailPage />} />
                    </Route>

                    {/* Payment pages */}
                    <Route path="payments">
                        <Route path="" element={<PaymentsPage />} />
                        <Route path=":id" element={<PaymentDetailPage />} />
                    </Route>
                </Route>
            </Route>

            <Route path="*" element={<NotFound />} />
        </Routes>
    );
};

export default AppRoutes;
