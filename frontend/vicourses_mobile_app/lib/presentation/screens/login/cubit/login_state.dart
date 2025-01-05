part of 'login_cubit.dart';

enum LoginStatus {
  pending,
  success,
  failed,
}

class LoginState {
  final LoginStatus status;
  final String? errorMessage;
  final LoginResponse? loginResponse;

  LoginState({
    this.status = LoginStatus.pending,
    this.errorMessage,
    this.loginResponse,
  });
}
