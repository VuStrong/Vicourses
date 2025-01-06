import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/category.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';

class CategoryService extends ApiService {
  Future<List<Category>> getCategories() async {
    try {
      final response = await dio.get('/api/cs/v1/categories');

      return (response.data as List).map((e) => Category.fromMap(e)).toList();
    } on DioException {
      return [];
    }
  }
}
