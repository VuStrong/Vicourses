import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'enrolled_courses_state.dart';

class EnrolledCoursesCubit extends Cubit<EnrolledCoursesState> {
  final _limit = 10;

  final CourseService _courseService;
  final String userId;

  EnrolledCoursesCubit(this._courseService, {required this.userId})
      : super(EnrolledCoursesState());

  Future<void> fetchEnrolledCourses() async {
    emit(EnrolledCoursesState(isLoading: true));

    final result = await _courseService.getUserEnrolledCourses(
      userId: userId,
      limit: _limit,
    );

    emit(EnrolledCoursesState(
      isLoading: false,
      courses: result?.items,
      end: result?.end ?? true,
    ));
  }

  Future<void> loadMore() async {
    if (state.isLoadingMore || state.isRefreshing) return;

    emit(state.copyWith(isLoadingMore: true));

    final skip = state.courses != null ? state.courses!.length : 0;

    final result = await _courseService.getUserEnrolledCourses(
      userId: userId,
      skip: skip,
      limit: _limit,
    );

    if (result != null) {
      final oldCourses = state.courses ?? [];
      final newCourses = [...oldCourses, ...result.items];

      emit(state.copyWith(
        end: result.end,
        isLoadingMore: false,
        courses: () => newCourses,
      ));
    }
  }

  Future<void> refresh() async {
    if (state.isLoadingMore || state.isRefreshing) return;

    emit(state.copyWith(isRefreshing: true));

    final result = await _courseService.getUserEnrolledCourses(
      userId: userId,
      limit: _limit,
    );

    if (result != null) {
      emit(state.copyWith(
        end: result.end,
        courses: () => result.items,
        isRefreshing: false,
      ));
    }
  }
}
