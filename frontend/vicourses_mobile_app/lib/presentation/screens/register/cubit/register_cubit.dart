import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/login_response.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'register_state.dart';

class RegisterCubit extends Cubit<RegisterState> {
  final AuthService _authService;

  RegisterCubit(this._authService) : super(RegisterState());

  Future<void> register({
    required String name,
    required String email,
    required String password,
  }) async {
    emit(state.copyWith(status: RegisterStatus.pending));

    try {
      await _authService.register(name: name, email: email, password: password);

      final loginResponse = await _authService.login(email, password);

      emit(state.copyWith(
        status: RegisterStatus.success,
        loginResponse: () => loginResponse,
        errorMessage: () => null,
      ));
    } on AppException catch (e) {
      emit(state.copyWith(
        status: RegisterStatus.failed,
        loginResponse: () => null,
        errorMessage: () => e.message,
      ));
    }
  }

  void togglePasswordObscure() {
    emit(state.copyWith(passwordObscure: !state.passwordObscure));
  }
}
