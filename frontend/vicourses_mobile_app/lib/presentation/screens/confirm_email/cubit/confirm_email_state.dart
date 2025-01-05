part of 'confirm_email_cubit.dart';

enum ConfirmEmailStatus {
  initial,
  sending,
  sent,
  sendFailed,
}

class ConfirmEmailState {
  final ConfirmEmailStatus status;
  final String? errorMessage;

  ConfirmEmailState({
    required this.status,
    this.errorMessage,
  });

  static ConfirmEmailState initial() {
    return ConfirmEmailState(status: ConfirmEmailStatus.initial);
  }
}
