part of 'user_bloc.dart';

abstract class UserEvent {}

class FetchUserEvent extends UserEvent {}

class LoginUserEvent extends UserEvent {
  final LoginResponse loginResponse;

  LoginUserEvent({required this.loginResponse});
}

class LogoutUserEvent extends UserEvent {}