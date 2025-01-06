import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/login_response.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'login_state.dart';

class LoginCubit extends Cubit<LoginState> {
  final AuthService _authService;

  LoginCubit(this._authService) : super(LoginState());

  Future<void> login(String email, String password) async {
    emit(state.copyWith(status: LoginStatus.pending));

    try {
      final loginResponse = await _authService.login(email, password);

      emit(state.copyWith(
        status: LoginStatus.success,
        loginResponse: () => loginResponse,
        errorMessage: () => null,
      ));
    } on AppException catch (e) {
      emit(state.copyWith(
        status: LoginStatus.failed,
        loginResponse: () => null,
        errorMessage: () => e.message,
      ));
    }
  }

  Future<void> loginWithGoogle(String idToken) async {
    emit(state.copyWith(status: LoginStatus.pending));

    try {
      final loginResponse = await _authService.loginWithGoogle(idToken);

      emit(state.copyWith(
        status: LoginStatus.success,
        loginResponse: () => loginResponse,
        errorMessage: () => null,
      ));
    } on AppException catch (e) {
      emit(state.copyWith(
        status: LoginStatus.failed,
        loginResponse: () => null,
        errorMessage: () => e.message,
      ));
    }
  }

  void togglePasswordObscure() {
    emit(state.copyWith(passwordObscure: !state.passwordObscure));
  }
}
