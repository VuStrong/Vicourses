class AppRoutes {
  static const splash = '/splash';

  static const login = '/login';
  static const register = '/register';
  static const confirmEmail = '/confirm-email';

  static const home = '/';

  static const search = '/search';

  static const courseDetail = '/course/:id';
  static String getCourseDetailRoute(String id) => '/course/$id';
}