class AppRoutes {
  static const splash = '/splash';

  static const login = '/login';
  static const register = '/register';
  static const confirmEmail = '/confirm-email';
  static const forgotPassword = '/forgot-password';

  static const home = '/';

  static const search = '/search';
  static const searchResults = '/search/results';

  static const study = '/study';

  static const wishlist = '/wishlist';

  static const accountOverview = '/account';
  static const accountSecurity = '/account/security';
  static const changePassword = '/account/password';
  static const editProfile = '/account/edit-profile';

  static String getCategoryRoute(String slug) => '/category/$slug';

  static String getCourseDetailRoute(String id) => '/course/$id';
  static String getCourseRatingsRoute(String id) => '/course/$id/ratings';
  static String getEditRatingRoute(String courseId) => '/course/$courseId/edit-rating';

  static String getUserProfileRoute(String id) => '/user/$id';
  static String getUserCoursesRoute(String id) => '/user/$id/courses';
}