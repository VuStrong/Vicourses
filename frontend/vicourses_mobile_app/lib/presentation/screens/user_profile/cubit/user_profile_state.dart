part of 'user_profile_cubit.dart';

class UserProfileState {
  final bool isLoading;
  final bool isLoadingCourses;
  final PublicProfile? profile;
  final List<Course>? courses;
  final int totalCourses;

  UserProfileState({
    this.isLoading = false,
    this.isLoadingCourses = false,
    this.profile,
    this.courses,
    this.totalCourses = 0,
  });

  UserProfileState copyWith({
    bool? isLoading,
    bool? isLoadingCourses,
    PublicProfile? Function()? profile,
    List<Course>? Function()? courses,
    int? totalCourses,
  }) {
    return UserProfileState(
      isLoading: isLoading ?? this.isLoading,
      isLoadingCourses: isLoadingCourses ?? this.isLoadingCourses,
      profile: profile != null ? profile() : this.profile,
      courses: courses != null ? courses() : this.courses,
      totalCourses: totalCourses ?? this.totalCourses,
    );
  }
}
