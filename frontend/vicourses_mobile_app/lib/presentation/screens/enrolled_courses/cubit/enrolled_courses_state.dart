part of 'enrolled_courses_cubit.dart';

class EnrolledCoursesState {
  final bool isLoading;
  final bool isLoadingMore;
  final bool isRefreshing;
  final List<Course>? courses;
  final bool end;

  EnrolledCoursesState({
    this.isLoading = false,
    this.isLoadingMore = false,
    this.isRefreshing = false,
    this.courses,
    this.end = true,
  });

  EnrolledCoursesState copyWith({
    bool? isLoading,
    bool? isLoadingMore,
    bool? isRefreshing,
    List<Course>? Function()? courses,
    bool? end,
  }) {
    return EnrolledCoursesState(
      isLoading: isLoading ?? this.isLoading,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      isRefreshing: isRefreshing ?? this.isRefreshing,
      courses: courses != null ? courses() : this.courses,
      end: end ?? this.end,
    );
  }
}
