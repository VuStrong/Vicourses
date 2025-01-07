import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/login_response.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';
import 'package:vicourses_mobile_app/utils/local_storage.dart';

part 'user_event.dart';
part 'user_state.dart';

class UserBloc extends Bloc<UserEvent, UserState> {
  final AuthService _authService;
  final UserService _userService;
  final LocalStorage _storage;

  UserBloc(this._authService, this._userService, this._storage)
      : super(UserState.init()) {
    on<FetchUserEvent>(_onFetchUser);
    on<LoginUserEvent>(_onLoginUser);
    on<LogoutUserEvent>(_onLogoutUser);
    on<UserUpdatedEvent>(_onUserUpdated);

    add(FetchUserEvent());
  }

  Future<void> _onFetchUser(
    FetchUserEvent event,
    Emitter<UserState> emit,
  ) async {
    emit(UserState(status: UserStatus.loading));

    try {
      final user = await _userService.getAuthenticatedUser();

      emit(UserState(
        status: UserStatus.authenticated,
        user: user,
      ));
    } on Exception {
      emit(UserState(status: UserStatus.unauthenticated));
    }
  }

  Future<void> _onLoginUser(
    LoginUserEvent event,
    Emitter<UserState> emit,
  ) async {
    emit(UserState(status: UserStatus.loading));

    await _storage.setString(
      key: 'access_token',
      value: event.loginResponse.accessToken,
    );
    await _storage.setString(
      key: 'refresh_token',
      value: event.loginResponse.refreshToken,
    );
    await _storage.setString(
      key: 'user_id',
      value: event.loginResponse.user.id,
    );

    DateTime expiredAt = DateTime.now()
        .add(const Duration(minutes: AppConstants.accessTokenLifeTime));
    await _storage.setDateTime(
      key: 'access_token_expired_at',
      value: expiredAt,
    );

    try {
      final user = await _userService.getAuthenticatedUser();

      emit(UserState(
        status: UserStatus.authenticated,
        user: user,
      ));
    } on Exception {
      emit(UserState(status: UserStatus.unauthenticated));
    }
  }

  Future<void> _onLogoutUser(
    LogoutUserEvent event,
    Emitter<UserState> emit,
  ) async {
    String? refreshToken = await _storage.getString('refresh_token');
    String? userId = await _storage.getString('user_id');

    if (refreshToken != null && userId != null) {
      _authService.logout(
        refreshToken: refreshToken,
        userId: userId,
      );
    }

    await _storage.deleteAll();

    emit(UserState(status: UserStatus.unauthenticated));
  }

  Future<void> _onUserUpdated(
    UserUpdatedEvent event,
    Emitter<UserState> emit,
  ) async {
    emit(UserState(
      status: UserStatus.authenticated,
      user: event.user,
    ));
  }
}
