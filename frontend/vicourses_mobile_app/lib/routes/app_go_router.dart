import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/scaffold_with_bottom_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_overview/account_overview_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_security/account_security_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_security/change_password_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/edit_profile_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/category/category_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/confirm_email/confirm_email_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/course_detail_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/course_ratings_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/user_rating.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/edit_rating_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/preview_video_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/forgot_password/forgot_password_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/home/home_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/learning_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/login/login_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/register/register_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/search_results_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/search_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/splash/splash_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/enrolled_courses/enrolled_courses_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/user_profile/user_courses_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/user_profile/user_profile_screen.dart';
import 'package:vicourses_mobile_app/presentation/screens/wishlist/wishlist_screen.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

final _rootNavigatorKey = GlobalKey<NavigatorState>();
final _shellNavigatorHomeKey = GlobalKey<NavigatorState>();
final _shellNavigatorSearchKey = GlobalKey<NavigatorState>();
final _shellNavigatorStudyKey = GlobalKey<NavigatorState>();
final _shellNavigatorWishlistKey = GlobalKey<NavigatorState>();
final _shellNavigatorAccountKey = GlobalKey<NavigatorState>();

class AppGoRouter {
  static final GoRouter router = GoRouter(
    navigatorKey: _rootNavigatorKey,
    initialLocation: AppRoutes.splash,
    redirect: (context, state) {
      final userState = context.read<UserBloc>().state;
      final status = userState.status;
      final isGuestRoute = guestRoutes.contains(state.uri.path);

      if (status == UserStatus.authenticated) {
        final emailConfirmed = userState.user!.emailConfirmed;
        final needToGoHome = isGuestRoute ||
            state.uri.path == AppRoutes.splash ||
            state.uri.path == AppRoutes.confirmEmail;

        if (!emailConfirmed) {
          return AppRoutes.confirmEmail;
        } else if (needToGoHome) {
          return AppRoutes.home;
        }
      } else if (status == UserStatus.unauthenticated && !isGuestRoute) {
        return AppRoutes.login;
      }

      return null;
    },
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
      GoRoute(
        path: '/learn/:id',
        builder: (_, state) => LearningScreen(
          courseId: state.pathParameters['id'] ?? '',
        ),
      ),
      StatefulShellRoute.indexedStack(
        builder: (_, __, navigationShell) {
          return ScaffoldWithBottomBar(navigationShell: navigationShell);
        },
        branches: [
          StatefulShellBranch(
            navigatorKey: _shellNavigatorHomeKey,
            routes: [
              GoRoute(
                path: '/',
                pageBuilder: (_, __) => const NoTransitionPage(
                  child: HomeScreen(),
                ),
              ),
              GoRoute(
                path: '/course/:id',
                builder: (_, state) => CourseDetailScreen(
                  courseId: state.pathParameters['id'] ?? '',
                ),
                routes: [
                  GoRoute(
                    path: 'ratings',
                    builder: (_, state) => CourseRatingsScreen(
                      courseId: state.pathParameters['id'] ?? '',
                    ),
                    parentNavigatorKey: _rootNavigatorKey,
                  ),
                  GoRoute(
                    path: 'edit-rating',
                    builder: (_, state) => EditRatingScreen(
                      courseId: state.pathParameters['id'] ?? '',
                      cubit: state.extra as UserRatingCubit,
                    ),
                    parentNavigatorKey: _rootNavigatorKey,
                  ),
                  GoRoute(
                    path: 'preview-video',
                    builder: (_, state) => PreviewVideoScreen(
                      course: state.extra as CourseDetail,
                    ),
                    parentNavigatorKey: _rootNavigatorKey,
                  ),
                ],
              ),
              GoRoute(
                path: '/category/:slug',
                builder: (_, state) => CategoryScreen(
                  slug: state.pathParameters['slug'] ?? '',
                ),
              ),
              GoRoute(
                path: '/user/:id',
                builder: (_, state) => UserProfileScreen(
                  userId: state.pathParameters['id'] ?? '',
                ),
                routes: [
                  GoRoute(
                    path: 'courses',
                    builder: (_, state) => UserCoursesScreen(
                      userId: state.pathParameters['id'] ?? '',
                    ),
                  ),
                ],
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorSearchKey,
            routes: [
              GoRoute(
                path: '/search',
                pageBuilder: (_, __) => const NoTransitionPage(
                  child: SearchScreen(),
                ),
                routes: [
                  GoRoute(
                    path: 'results',
                    builder: (_, state) => SearchResultsScreen(
                      searchValue: state.uri.queryParameters['q'] ?? '',
                    ),
                  ),
                ],
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorStudyKey,
            routes: [
              GoRoute(
                path: '/study',
                pageBuilder: (_, __) => const NoTransitionPage(
                  child: EnrolledCoursesScreen(),
                ),
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorWishlistKey,
            routes: [
              GoRoute(
                path: '/wishlist',
                pageBuilder: (_, __) => const NoTransitionPage(
                  child: WishlistScreen(),
                ),
              ),
            ],
          ),
          StatefulShellBranch(
            navigatorKey: _shellNavigatorAccountKey,
            routes: [
              GoRoute(
                path: '/account',
                pageBuilder: (_, __) => const NoTransitionPage(
                  child: AccountOverviewScreen(),
                ),
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
                ],
              ),
            ],
          ),
        ],
      ),
    ],
  );

  static final List<String> guestRoutes = [
    AppRoutes.login,
    AppRoutes.register,
    AppRoutes.forgotPassword,
  ];
}
