import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/scaffold_app_with_bottom_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_overview/account_overview_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_security/account_security_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_security/change_password_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/edit_profile_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/confirm_email/confirm_email_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/course_detail_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/forgot_password/forgot_password_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/home/home_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/login/login_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/register/register_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/search_results_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/search_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/splash/splash_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/study/study_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/wishlist/wishlist_screen.dart';
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
        path: '/splash',
        builder: (_, __) => const SplashScreen(),
      ),
      GoRoute(
        path: '/login',
        builder: (_, __) => LoginScreen(),
      ),
      GoRoute(
        path: '/register',
        builder: (_, __) => RegisterScreen(),
      ),
      GoRoute(
        path: '/confirm-email',
        builder: (_, __) => const ConfirmEmailScreen(),
      ),
      GoRoute(
        path: '/forgot-password',
        builder: (_, __) => ForgotPasswordScreen(),
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
            path: '/',
            builder: (_, __) => const HomeScreen(),
          ),
          GoRoute(
            path: '/search',
            builder: (_, __) => const SearchScreen(),
          ),
          GoRoute(
            path: '/search-results',
            builder: (_, state) => SearchResultsScreen(
              searchValue: state.uri.queryParameters['q'] ?? '',
            ),
          ),
          GoRoute(
            path: '/course/:id',
            builder: (_, state) => CourseDetailScreen(
              id: state.pathParameters['id'] ?? '',
            ),
          ),
          GoRoute(
            path: '/study',
            builder: (_, __) => const StudyScreen(),
          ),
          GoRoute(
            path: '/wishlist',
            builder: (_, __) => const WishlistScreen(),
          ),
          GoRoute(
            path: '/account',
            builder: (_, __) => const AccountOverviewScreen(),
            routes: [
              GoRoute(
                path: 'security',
                builder: (_, __) => const AccountSecurityScreen(),
                parentNavigatorKey: _rootNavigatorKey,
              ),
              GoRoute(
                path: 'password',
                builder: (_, __) => const ChangePasswordScreen(),
                parentNavigatorKey: _rootNavigatorKey,
              ),
              GoRoute(
                path: 'edit-profile',
                builder: (_, __) => const EditProfileScreen(),
                parentNavigatorKey: _rootNavigatorKey,
              ),
            ]
          ),
        ],
      ),
    ],
  );
}
