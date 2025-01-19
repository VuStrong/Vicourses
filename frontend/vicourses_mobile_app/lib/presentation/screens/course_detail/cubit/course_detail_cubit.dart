import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'course_detail_state.dart';

class CourseDetailCubit extends Cubit<CourseDetailState> {
  final CourseService _courseService;

  CourseDetailCubit(this._courseService) : super(CourseDetailState());

  Future<void> fetchCourse(String courseId) async {
    emit(CourseDetailState(isLoading: true));

    final course = await _courseService.getCourse(courseId);

    emit(CourseDetailState(isLoading: false, course: course));

    checkEnroll();
  }

  Future<void> checkEnroll() async {
    if (state.course == null) return;

    emit(state.copyWith(isCheckingEnroll: true));

    final enrolled = await _courseService.checkEnroll(state.course!.id);

    emit(state.copyWith(isCheckingEnroll: false, enrolled: enrolled));
  }

  Future<void> enroll() async {
    if (state.course == null ||
        state.enrolled ||
        state.enrollingStatus == EnrollingStatus.pending) return;

    emit(state.copyWith(
      enrollingStatus: EnrollingStatus.pending,
    ));

    try {
      await _courseService.enrollInFreeCourse(state.course!.id);

      emit(state.copyWith(
        enrolled: true,
        enrollingStatus: EnrollingStatus.success,
      ));
    } on Exception {
      emit(state.copyWith(
        enrollingStatus: EnrollingStatus.failed,
      ));
    }
  }
}
