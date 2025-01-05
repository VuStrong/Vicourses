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
    emit(RegisterState(status: RegisterStatus.pending));

    try {
      await _authService.register(name: name, email: email, password: password);

      final loginResponse = await _authService.login(email, password);

      emit(RegisterState(
        status: RegisterStatus.success,
        loginResponse: loginResponse,
      ));
    } on AppException catch (e) {
      emit(RegisterState(
        status: RegisterStatus.failed,
        errorMessage: e.message,
      ));
    }
  }
}
