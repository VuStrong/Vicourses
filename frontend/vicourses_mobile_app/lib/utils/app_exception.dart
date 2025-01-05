class AppException implements Exception {
  final String? message;
  final int statusCode;

  AppException({
    this.message,
    required this.statusCode,
  });
}
