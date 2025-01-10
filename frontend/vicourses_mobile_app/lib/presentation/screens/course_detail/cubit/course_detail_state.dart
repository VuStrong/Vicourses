part of 'course_detail_cubit.dart';

class CourseDetailState {
  final bool isLoading;
  final bool isCheckingEnroll;
  final bool enrolled;
  final CourseDetail? course;

  CourseDetailState({
    this.isLoading = false,
    this.isCheckingEnroll = false,
    this.enrolled = false,
    this.course,
  });

  CourseDetailState copyWith({
    bool? isLoading,
    bool? isCheckingEnroll,
    bool? enrolled,
    CourseDetail? Function()? course,
  }) {
    return CourseDetailState(
      isLoading: isLoading ?? this.isLoading,
      isCheckingEnroll: isCheckingEnroll ?? this.isCheckingEnroll,
      enrolled: enrolled ?? this.enrolled,
      course: course != null ? course() : this.course,
    );
  }
}
