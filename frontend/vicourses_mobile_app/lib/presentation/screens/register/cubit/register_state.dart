part of 'register_cubit.dart';

enum RegisterStatus {
  initial,
  pending,
  success,
  failed,
}

class RegisterState {
  final RegisterStatus status;
  final String? errorMessage;
  final LoginResponse? loginResponse;
  final bool passwordObscure;

  RegisterState({
    this.status = RegisterStatus.initial,
    this.errorMessage,
    this.loginResponse,
    this.passwordObscure = true,
  });

  RegisterState copyWith({
    RegisterStatus? status,
    String? Function()? errorMessage,
    LoginResponse? Function()? loginResponse,
    bool? passwordObscure,
  }) {
    return RegisterState(
      status: status ?? this.status,
      errorMessage: errorMessage != null ? errorMessage() : this.errorMessage,
      loginResponse: loginResponse != null ? loginResponse() : this.loginResponse,
      passwordObscure: passwordObscure ?? this.passwordObscure,
    );
  }
}
