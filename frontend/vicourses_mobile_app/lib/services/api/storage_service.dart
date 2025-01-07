import 'dart:io';

import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/upload_response.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

class StorageService extends ApiService {
  Future<UploadResponse> uploadImage(File image, {String? fileId}) async {
    final formData = FormData.fromMap({
      'image': await MultipartFile.fromFile(
        image.path,
        filename: fileId,
        contentType: DioMediaType('image','jpeg'),
      ),
      if (fileId != null) 'fileId': fileId,
    });

    try {
      final response = await dio.post(
        '/api/sts/v1/upload-image',
        data: formData,
      );

      return UploadResponse.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
