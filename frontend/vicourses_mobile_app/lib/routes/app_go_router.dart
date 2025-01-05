import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/scaffold_app_with_bottom_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/confirm_email/confirm_email_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/course_detail_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/home/home_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/login/login_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/register/register_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/search_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/splash/splash_screen.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

final GlobalKey<NavigatorState> _rootNavigatorKey = GlobalKey<NavigatorState>();
final GlobalKey<NavigatorState> _shellNavigatorKey =
    GlobalKey<NavigatorState>();

class AppGoRouter {
  static final GoRouter router = GoRouter(
    navigatorKey: _rootNavigatorKey,
    initialLocation: AppRoutes.home,
    routes: [
      GoRoute(
        path: AppRoutes.splash,
        builder: (_, __) => const SplashScreen(),
      ),
      GoRoute(
        path: AppRoutes.login,
        builder: (_, __) => LoginScreen(),
      ),
      GoRoute(
        path: AppRoutes.register,
        builder: (_, __) => RegisterScreen(),
      ),
      GoRoute(
        path: AppRoutes.confirmEmail,
        builder: (_, __) => const ConfirmEmailScreen(),
      ),
      ShellRoute(
        navigatorKey: _shellNavigatorKey,
        builder: (_, state, child) {
          return ScaffoldAppWithBottomBar(
            location: state.matchedLocation,
            child: child,
          );
        },
        routes: [
          GoRoute(
            path: AppRoutes.home,
            builder: (_, __) => const HomeScreen(),
          ),
          GoRoute(
            path: AppRoutes.search,
            builder: (_, __) => const SearchScreen(),
          ),
          GoRoute(
            path: AppRoutes.courseDetail,
            builder: (_, state) => CourseDetailScreen(
              id: state.pathParameters['id'] ?? '',
            ),
          ),
        ],
      ),
    ],
  );
}
