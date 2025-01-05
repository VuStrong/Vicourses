import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'confirm_email_state.dart';

class ConfirmEmailCubit extends Cubit<ConfirmEmailState> {
  final AuthService _authService;

  ConfirmEmailCubit(this._authService) : super(ConfirmEmailState.initial());

  Future<void> resendEmail(String email) async {
    emit(ConfirmEmailState(status: ConfirmEmailStatus.sending));

    try {
      await _authService.sendEmailConfirmationLink(email);

      emit(ConfirmEmailState(status: ConfirmEmailStatus.sent));
    } on AppException catch (e) {
      emit(ConfirmEmailState(
        status: ConfirmEmailStatus.sendFailed,
        errorMessage: e.message,
      ));
    }
  }
}
