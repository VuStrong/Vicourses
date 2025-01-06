import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'forgot_password_state.dart';

class ForgotPasswordCubit extends Cubit<ForgotPasswordState> {
  final AuthService _authService;

  ForgotPasswordCubit(this._authService) : super(ForgotPasswordState());

  Future<void> sendPasswordResetLink(String email) async {
    emit(ForgotPasswordState(status: SendPasswordResetLinkStatus.sending));

    try {
      await _authService.sendPasswordResetLink(email);

      emit(ForgotPasswordState(status: SendPasswordResetLinkStatus.sent));
    } on AppException catch (e) {
      emit(ForgotPasswordState(
        status: SendPasswordResetLinkStatus.sendFailed,
        errorMessage: e.message,
      ));
    }
  }
}
