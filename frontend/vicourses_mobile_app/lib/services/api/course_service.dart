import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/paged_result.dart';

class CourseService extends ApiService {
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
}
