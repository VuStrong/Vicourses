import 'dart:convert';
import 'package:dio/dio.dart';

import 'package:vicourses_mobile_app/models/login_response.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

class AuthService extends ApiService {
  Future<LoginResponse> login(String email, String password) async {
    String body = jsonEncode({
      'email': email,
      'password': password,
    });

    try {
      final response = await dio.post(
        '/api/us/v1/auth/login',
        data: body,
      );

      return LoginResponse.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<LoginResponse> loginWithGoogle(String idToken) async {
    String body = jsonEncode({
      'idToken': idToken,
    });

    try {
      final response = await dio.post(
        '/api/us/v1/auth/google-login',
        data: body,
      );

      return LoginResponse.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<User> register({
    required String name,
    required String email,
    required String password,
  }) async {
    String body = jsonEncode({
      'name': name,
      'email': email,
      'password': password,
    });

    try {
      final response = await dio.post(
        '/api/us/v1/auth/register',
        data: body,
      );

      return User.fromMap(response.data['user']);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> logout({
    required String refreshToken,
    required String userId,
  }) async {
    String body = jsonEncode({
      'refreshToken': refreshToken,
      'userId': userId,
    });

    try {
      await dio.post(
        '/api/us/v1/auth/revoke-refresh-token',
        data: body,
      );
    } on DioException {
      //
    }
  }

  Future<void> sendEmailConfirmationLink(String email) async {
    String body = jsonEncode({
      'email': email,
    });

    try {
      await dio.post(
        '/api/us/v1/auth/email-confirmation-link',
        data: body,
      );
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> sendPasswordResetLink(String email) async {
    String body = jsonEncode({
      'email': email,
    });

    try {
      await dio.post(
        '/api/us/v1/auth/password-reset-link',
        data: body,
      );
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
