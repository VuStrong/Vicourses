part of 'user_bloc.dart';

enum UserStatus {
  loading,
  authenticated,
  unauthenticated,
}

class UserState {
  final User? user;
  final UserStatus status;

  UserState({
    required this.status,
    this.user,
  });

  static UserState init() {
    return UserState(status: UserStatus.loading);
  }
}
