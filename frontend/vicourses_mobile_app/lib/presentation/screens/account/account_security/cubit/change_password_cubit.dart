import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'change_password_state.dart';

class ChangePasswordCubit extends Cubit<ChangePasswordState> {
  final UserService _userService;

  ChangePasswordCubit(this._userService) : super(ChangePasswordState());

  Future<void> changePassword({
    required String oldPassword,
    required String newPassword,
  }) async {
    emit(state.copyWith(status: ChangePasswordStatus.pending));

    try {
      await _userService.changePassword(
        oldPassword: oldPassword,
        newPassword: newPassword,
      );

      emit(state.copyWith(
        status: ChangePasswordStatus.success,
        errorMessage: () => null,
      ));
    } on AppException catch (e) {
      emit(state.copyWith(
        status: ChangePasswordStatus.failed,
        errorMessage: () => e.message,
      ));
    }
  }

  void toggleOldPasswordObscure() {
    emit(state.copyWith(oldPasswordObscure: !state.oldPasswordObscure));
  }

  void toggleNewPasswordObscure() {
    emit(state.copyWith(newPasswordObscure: !state.newPasswordObscure));
  }
}
