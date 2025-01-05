import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

class UserService extends ApiService {
  Future<User> getAuthenticatedUser({
    String? fields,
  }) async {
    try {
      final response = await dio.get('/api/us/v1/me', queryParameters: {
        if (fields != null) 'fields': fields,
      });

      return User.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
