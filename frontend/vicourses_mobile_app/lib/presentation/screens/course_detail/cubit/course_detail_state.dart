part of 'course_detail_cubit.dart';

enum EnrollingStatus { idle, pending, success, failed }

class CourseDetailState {
  final bool isLoading;
  final bool isCheckingEnroll;
  final bool enrolled;
  final CourseDetail? course;
  final EnrollingStatus enrollingStatus;

  CourseDetailState({
    this.isLoading = false,
    this.isCheckingEnroll = false,
    this.enrolled = false,
    this.course,
    this.enrollingStatus = EnrollingStatus.idle,
  });

  CourseDetailState copyWith({
    bool? isLoading,
    bool? isCheckingEnroll,
    bool? enrolled,
    CourseDetail? Function()? course,
    EnrollingStatus? enrollingStatus,
  }) {
    return CourseDetailState(
      isLoading: isLoading ?? this.isLoading,
      isCheckingEnroll: isCheckingEnroll ?? this.isCheckingEnroll,
      enrolled: enrolled ?? this.enrolled,
      course: course != null ? course() : this.course,
      enrollingStatus: enrollingStatus ?? this.enrollingStatus,
    );
  }
}
