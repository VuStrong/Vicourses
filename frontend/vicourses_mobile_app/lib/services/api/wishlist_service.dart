import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/wishlist.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

class WishlistService extends ApiService {
  Future<Wishlist?> getWishlist() async {
    try {
      final response = await dio.get('/api/ws/v1/wishlist');

      return Wishlist.fromMap(response.data);
    } on DioException {
      return null;
    }
  }

  Future<Wishlist> addToWishlist(String courseId) async {
    try {
      final response = await dio.post('/api/ws/v1/wishlist/courses/$courseId');

      return Wishlist.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<Wishlist> removeFromWishlist(String courseId) async {
    try {
      final response = await dio.delete('/api/ws/v1/wishlist/courses/$courseId');

      return Wishlist.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
