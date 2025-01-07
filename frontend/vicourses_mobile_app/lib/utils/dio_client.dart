import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:vicourses_mobile_app/services/api/interceptors/token_interceptor.dart';
import 'package:vicourses_mobile_app/utils/local_storage.dart';

class DioClient {
  static Dio? _dio;

  static Dio getDio() {
    if (_dio != null) return _dio!;

    _dio = Dio();
    _dio!.interceptors.add(TokenInterceptor(LocalStorage()));
    _dio!.options.baseUrl = dotenv.env['BACKEND_URL'] ?? '';

    return _dio!;
  }
}