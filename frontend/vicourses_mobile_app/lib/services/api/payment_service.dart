import 'dart:convert';

import 'package:dio/dio.dart';
import 'package:vicourses_mobile_app/models/payment.dart';
import 'package:vicourses_mobile_app/services/api/api_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

class PaymentService extends ApiService {
  Future<Payment> createPaypalPayment(String courseId) async {
    String body = jsonEncode({
      'courseId': courseId,
    });

    try {
      final response = await dio.post('/api/ps/v1/payments/paypal', data: body);

      return Payment.fromMap(response.data);
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }

  Future<void> capturePaypalPayment(String paypalOrderId) async {
    try {
      await dio.post('/api/ps/v1/payments/paypal/$paypalOrderId/capture');
    } on DioException catch (e) {
      throw AppException(
        statusCode: e.response?.statusCode ?? 400,
        message: e.response?.data?['message'],
      );
    }
  }
}
