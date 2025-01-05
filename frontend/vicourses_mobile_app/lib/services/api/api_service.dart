import 'package:dio/dio.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:vicourses_mobile_app/services/api/interceptors/token_interceptor.dart';
import 'package:vicourses_mobile_app/utils/local_storage.dart';

abstract class ApiService {
  late final Dio dio;

  ApiService() {
    dio = Dio();
    dio.interceptors.add(TokenInterceptor(LocalStorage()));
    dio.options.baseUrl = dotenv.env['BACKEND_URL'] ?? '';
  }
}