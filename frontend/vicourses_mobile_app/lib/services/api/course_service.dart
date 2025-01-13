import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/models/lesson.dart';
import 'package:vicourses_mobile_app/models/public_curriculum.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';
import 'package:vicourses_mobile_app/utils/paged_result.dart';

class CourseService extends ApiService {
  Future<PagedResult<Course>?> getCourses({
    int skip = 0,
    int limit = 15,
    String? categoryId,
    String? subCategoryId,
    String? sort,
    num? rating,
    String? level,
    bool? free,
  }) async {
    try {
      final response = await dio.get('/api/cs/v1/courses', queryParameters: {
        'skip': skip,
        'limit': limit,
        if (categoryId != null) 'categoryId': categoryId,
        if (subCategoryId != null) 'subCategoryId': subCategoryId,
        if (sort != null) 'sort': sort,
        if (rating != null) 'rating': rating,
        if (level != null) 'level': level,
        if (free != null) 'free': free,
      });
      final data = response.data;

      return PagedResult<Course>(
        items: (data['items'] as List).map((e) => Course.fromMap(e)).toList(),
        total: data['total'],
        skip: data['skip'],
        limit: data['limit'],
        end: data['end'],
      );
    } on DioException {
      return null;
    }
  }

  Future<CourseDetail?> getCourse(String courseId) async {
    try {
      final response = await dio.get('/api/cs/v1/courses/$courseId');

      return CourseDetail.fromMap(response.data);
    } on DioException {
      return null;
    }
  }

  Future<PagedResult<Course>?> searchCourses({
    String? q,
    int skip = 0,
    int limit = 15,
    String? sort,
    num? rating,
    String? level,
    bool? free,
  }) async {
    try {
      final response = await dio.get('/api/ss/v1/search', queryParameters: {
        if (q != null) 'q': q,
        'skip': skip,
        'limit': limit,
        if (sort != null) 'sort': sort,
        if (rating != null) 'rating': rating,
        if (level != null) 'level': level,
        if (free != null) 'free': free,
      });
      final data = response.data;

      return PagedResult<Course>(
        items: (data['items'] as List).map((e) => Course.fromMap(e)).toList(),
        total: data['total'],
        skip: data['skip'],
        limit: data['limit'],
        end: data['end'],
      );
    } on DioException {
      return null;
    }
  }

  Future<PagedResult<Course>?> getInstructorCourses({
    required String instructorId,
    String? q,
    int skip = 0,
    int limit = 15,
    String? status,
  }) async {
    try {
      final response = await dio.get(
        '/api/cs/v1/courses/instructor-courses',
        queryParameters: {
          if (q != null) 'q': q,
          'instructorId': instructorId,
          'skip': skip,
          'limit': limit,
          if (status != null) 'status': status,
        },
      );
      final data = response.data;

      return PagedResult<Course>(
        items: (data['items'] as List).map((e) => Course.fromMap(e)).toList(),
        total: data['total'],
        skip: data['skip'],
        limit: data['limit'],
        end: data['end'],
      );
    } on DioException {
      return null;
    }
  }

  Future<PagedResult<Course>?> getUserEnrolledCourses({
    required String userId,
    int skip = 0,
    int limit = 15,
  }) async {
    try {
      final response = await dio.get(
        '/api/cs/v1/courses/enrolled-courses',
        queryParameters: {
          'userId': userId,
          'skip': skip,
          'limit': limit,
        },
      );
      final data = response.data;

      return PagedResult<Course>(
        items: (data['items'] as List).map((e) => Course.fromMap(e)).toList(),
        total: data['total'],
        skip: data['skip'],
        limit: data['limit'],
        end: data['end'],
      );
    } on DioException {
      return null;
    }
  }

  Future<PublicCurriculum?> getPublicCurriculum(String courseId) async {
    try {
      final response = await dio.get('/api/cs/v1/courses/$courseId/public-curriculum');

      return PublicCurriculum.fromMap(response.data);
    } on DioException {
      return null;
    }
  }

  Future<Lesson?> getLesson(String lessonId) async {
    try {
      final response = await dio.get('/api/cs/v1/lessons/$lessonId');

      return Lesson.fromMap(response.data);
    } on DioException {
      return null;
    }
  }

  Future<bool> checkEnroll(String courseId) async {
    try {
      await dio.head('/api/cs/v1/courses/$courseId/enroll');

      return true;
    } on DioException {
      return false;
    }
  }

  Future<void> enrollInFreeCourse(String courseId) async {
    try {
      await dio.post('/api/cs/v1/courses/$courseId/enroll');
    } on DioException catch(e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
