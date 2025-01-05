import 'package:vicourses_mobile_app/models/user.dart';

class LoginResponse {
  final String accessToken;
  final String refreshToken;
  final User user;

  LoginResponse({
    required this.accessToken,
    required this.refreshToken,
    required this.user,
  });

  static LoginResponse fromMap(Map<String, dynamic> data) {
    return LoginResponse(
      accessToken: data['accessToken'],
      refreshToken: data['refreshToken'],
      user: User.fromMap(data['user']),
    );
  }
}
