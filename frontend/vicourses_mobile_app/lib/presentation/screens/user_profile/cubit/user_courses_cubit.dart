import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

part 'user_courses_state.dart';

class UserCoursesCubit extends Cubit<UserCoursesState> {
  final _limit = 10;

  final CourseService _courseService;
  final String userId;

  UserCoursesCubit(this._courseService, {required this.userId}) : super(UserCoursesState());

  Future<void> fetchCourses() async {
    emit(UserCoursesState(isLoading: true));

    final result = await _courseService.getInstructorCourses(
      instructorId: userId,
      limit: _limit,
      status: CourseStatus.published,
    );

    emit(UserCoursesState(
      isLoading: false,
      courses: result?.items,
      totalCourses: result?.total ?? 0,
      end: result?.end ?? true,
    ));
  }

  Future<void> loadMore() async {
    final skip = state.courses != null ? state.courses!.length : 0;

    final result = await _courseService.getInstructorCourses(
      instructorId: userId,
      skip: skip,
      limit: _limit,
      status: CourseStatus.published,
    );

    if (result != null) {
      state.courses?.addAll(result.items);

      emit(state.copyWith(
        end: result.end,
      ));
    }
  }
}
