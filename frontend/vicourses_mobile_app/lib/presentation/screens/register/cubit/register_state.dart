part of 'register_cubit.dart';


enum RegisterStatus {
  pending,
  success,
  failed,
}

class RegisterState {
  final RegisterStatus status;
  final String? errorMessage;
  final LoginResponse? loginResponse;

  RegisterState({
    this.status = RegisterStatus.pending,
    this.errorMessage,
    this.loginResponse,
  });
}