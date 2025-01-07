import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';

part 'user_profile_state.dart';

class UserProfileCubit extends Cubit<UserProfileState> {
  final UserService _userService;

  UserProfileCubit(this._userService) : super(UserProfileState());

  Future<void> fetchProfile(String id) async {
    emit(UserProfileState(isLoading: true));

    final profile = await _userService.getPublicProfile(id);

    emit(UserProfileState(isLoading: false, profile: profile));
  }
}