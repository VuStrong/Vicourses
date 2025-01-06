part of 'forgot_password_cubit.dart';

enum SendPasswordResetLinkStatus {
  initial,
  sending,
  sent,
  sendFailed,
}

class ForgotPasswordState {
  final SendPasswordResetLinkStatus status;
  final String? errorMessage;

  ForgotPasswordState({
    this.status = SendPasswordResetLinkStatus.initial,
    this.errorMessage,
  });
}