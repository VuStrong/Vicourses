import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/utils/dio_client.dart';

abstract class ApiService {
  late final Dio dio;

  ApiService() {
    dio = DioClient.getDio();
  }
}
