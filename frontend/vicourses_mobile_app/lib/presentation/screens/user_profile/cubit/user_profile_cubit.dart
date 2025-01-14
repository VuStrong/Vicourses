import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

part 'user_profile_state.dart';

class UserProfileCubit extends Cubit<UserProfileState> {
  final UserService _userService;
  final CourseService _courseService;

  UserProfileCubit(this._userService, this._courseService)
      : super(UserProfileState());

  Future<void> fetchProfile(String id) async {
    emit(UserProfileState(isLoading: true));

    final profile = await _userService.getPublicProfile(id);

    emit(UserProfileState(isLoading: false, profile: profile));
  }

  Future<void> fetchCourses({int count = 3}) async {
    if (state.profile == null) return;

    emit(state.copyWith(isLoadingCourses: true));

    final result = await _courseService.getInstructorCourses(
      instructorId: state.profile!.id,
      limit: count,
      status: CourseStatus.published,
    );

    emit(state.copyWith(
      isLoadingCourses: false,
      courses: () => result?.items,
      totalCourses: result?.total ?? 0,
    ));
  }
}
