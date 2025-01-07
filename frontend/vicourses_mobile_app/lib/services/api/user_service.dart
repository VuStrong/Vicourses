import 'dart:convert';

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

  Future<void> changePassword({
    required String oldPassword,
    required String newPassword,
  }) async {
    String body = jsonEncode({
      'oldPassword': oldPassword,
      'newPassword': newPassword,
    });

    try {
      await dio.patch('/api/us/v1/me/password', data: body);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<User> editProfile({
    String? name,
    String? thumbnailToken,
    String? headline,
    String? description,
    String? websiteUrl,
    String? youtubeUrl,
    String? facebookUrl,
    String? linkedInUrl,
    bool? enrolledCoursesVisible,
    bool? isPublic,
  }) async {
    String body = jsonEncode({
      if (name != null) 'name': name,
      if (thumbnailToken != null) 'thumbnailToken': thumbnailToken,
      if (headline != null) 'headline': headline,
      if (description != null) 'description': description,
      if (websiteUrl != null) 'websiteUrl': websiteUrl,
      if (youtubeUrl != null) 'youtubeUrl': youtubeUrl,
      if (facebookUrl != null) 'facebookUrl': facebookUrl,
      if (linkedInUrl != null) 'linkedInUrl': linkedInUrl,
      if (enrolledCoursesVisible != null) 'enrolledCoursesVisible': enrolledCoursesVisible,
      if (isPublic != null) 'isPublic': isPublic,
    });

    try {
      final response = await dio.patch('/api/us/v1/me', data: body);

      return User.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<PublicProfile?> getPublicProfile(String id) async {
    try {
      final response = await dio.get('/api/us/v1/users/$id/public-profile');

      return PublicProfile.fromMap(response.data);
    } on Exception {
      return null;
    }
  }
}
