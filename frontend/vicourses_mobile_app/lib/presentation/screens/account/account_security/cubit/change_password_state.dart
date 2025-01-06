part of 'change_password_cubit.dart';

enum ChangePasswordStatus { initial, pending, success, failed }

class ChangePasswordState {
  final ChangePasswordStatus status;
  final String? errorMessage;
  final bool oldPasswordObscure;
  final bool newPasswordObscure;

  ChangePasswordState({
    this.status = ChangePasswordStatus.initial,
    this.errorMessage,
    this.oldPasswordObscure = true,
    this.newPasswordObscure = true,
  });

  ChangePasswordState copyWith({
    ChangePasswordStatus? status,
    String? Function()? errorMessage,
    bool? oldPasswordObscure,
    bool? newPasswordObscure,
  }) {
    return ChangePasswordState(
      status: status ?? this.status,
      errorMessage: errorMessage?.call() ?? this.errorMessage,
      oldPasswordObscure: oldPasswordObscure ?? this.oldPasswordObscure,
      newPasswordObscure: newPasswordObscure ?? this.newPasswordObscure,
    );
  }
}
