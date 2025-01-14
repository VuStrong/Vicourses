import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/comment.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';
import 'package:vicourses_mobile_app/utils/paged_result.dart';

class CommentService extends ApiService {
  Future<PagedResult<Comment>?> getComments(
    String lessonId, {
    int skip = 0,
    int limit = 10,
    String? sort,
    String? replyToId,
  }) async {
    try {
      final response = await dio.get(
        '/api/cs/v1/lessons/$lessonId/comments',
        queryParameters: {
          'skip': skip,
          'limit': limit,
          if (sort != null) 'sort': sort,
          if (replyToId != null) 'replyToId': replyToId,
        },
      );
      final data = response.data;

      return PagedResult<Comment>(
        items: (data['items'] as List).map((e) => Comment.fromMap(e)).toList(),
        total: data['total'],
        skip: data['skip'],
        limit: data['limit'],
        end: data['end'],
      );
    } on DioException {
      return null;
    }
  }

  Future<Comment> createComment({
    required String lessonId,
    required String content,
    String? replyToId,
  }) async {
    String body = jsonEncode({
      'content': content,
      if (replyToId != null) 'replyToId': replyToId,
    });

    try {
      final response = await dio.post(
        '/api/cs/v1/lessons/$lessonId/comments',
        data: body,
      );

      return Comment.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> deleteComment(String lessonId, String commentId) async {
    try {
      await dio.delete('/api/cs/v1/lessons/$lessonId/comments/$commentId');
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> upvoteComment(String lessonId, String commentId) async {
    try {
      await dio.post('/api/cs/v1/lessons/$lessonId/comments/$commentId/upvote');
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> cancelUpvoteComment(String lessonId, String commentId) async {
    try {
      await dio.post(
          '/api/cs/v1/lessons/$lessonId/comments/$commentId/cancel-upvote');
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
