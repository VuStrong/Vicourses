import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/rating.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';
import 'package:vicourses_mobile_app/utils/paged_result.dart';

class RatingService extends ApiService {
  Future<PagedResult<Rating>?> getCourseRatings(
    String courseId, {
    int skip = 0,
    int limit = 10,
    int? star,
    bool? responded,
  }) async {
    try {
      final response = await dio.get('/api/rs/v1/ratings', queryParameters: {
        'courseId': courseId,
        'skip': skip,
        'limit': limit,
        if (star != null) 'star': star,
        if (responded != null) 'responded': responded,
      });
      final data = response.data;

      return PagedResult<Rating>(
        items: (data['items'] as List).map((e) => Rating.fromMap(e)).toList(),
        total: data['total'],
        skip: data['skip'],
        limit: data['limit'],
        end: data['end'],
      );
    } on DioException {
      return null;
    }
  }

  Future<Rating?> getUserRating(String courseId) async {
    try {
      final response = await dio.get('/api/rs/v1/ratings/me', queryParameters: {
        'courseId': courseId,
      });

      return Rating.fromMap(response.data);
    } on DioException {
      return null;
    }
  }

  Future<Rating> createRating({
    required String courseId,
    required String feedback,
    required int star,
  }) async {
    String body = jsonEncode({
      'courseId': courseId,
      'feedback': feedback,
      'star': star,
    });

    try {
      final response = await dio.post('/api/rs/v1/ratings', data: body);

      return Rating.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<Rating> updateRating(
    String ratingId, {
    required String feedback,
    required int star,
  }) async {
    String body = jsonEncode({
      'feedback': feedback,
      'star': star,
    });

    try {
      final response = await dio.patch(
        '/api/rs/v1/ratings/$ratingId',
        data: body,
      );

      return Rating.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> deleteRating(String ratingId) async {
    try {
      await dio.delete('/api/rs/v1/ratings/$ratingId');
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
