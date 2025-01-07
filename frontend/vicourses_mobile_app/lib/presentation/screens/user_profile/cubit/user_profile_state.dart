part of 'user_profile_cubit.dart';

class UserProfileState {
  final bool isLoading;
  final PublicProfile? profile;

  UserProfileState({
    this.isLoading = false,
    this.profile,
  });
}
