part of 'login_cubit.dart';

enum LoginStatus {
  initial,
  pending,
  success,
  failed,
}

class LoginState {
  final LoginStatus status;
  final String? errorMessage;
  final LoginResponse? loginResponse;
  final bool passwordObscure;

  LoginState({
    this.status = LoginStatus.initial,
    this.errorMessage,
    this.loginResponse,
    this.passwordObscure = true,
  });

  LoginState copyWith({
    LoginStatus? status,
    String? Function()? errorMessage,
    LoginResponse? Function()? loginResponse,
    bool? passwordObscure,
  }) {
    return LoginState(
      status: status ?? this.status,
      errorMessage: errorMessage != null ? errorMessage() : this.errorMessage,
      loginResponse: loginResponse != null ? loginResponse() : this.loginResponse,
      passwordObscure: passwordObscure ?? this.passwordObscure,
    );
  }
}
