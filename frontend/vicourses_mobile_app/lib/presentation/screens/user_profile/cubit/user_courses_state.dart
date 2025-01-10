part of 'user_courses_cubit.dart';

class UserCoursesState {
  final bool isLoading;
  final List<Course>? courses;
  final int totalCourses;
  final bool end;

  UserCoursesState({
    this.isLoading = false,
    this.courses,
    this.totalCourses = 0,
    this.end = true,
  });

  UserCoursesState copyWith({
    bool? isLoading,
    List<Course>? Function()? courses,
    int? totalCourses,
    bool? end,
}) {
    return UserCoursesState(
      isLoading: isLoading ?? this.isLoading,
      courses: courses != null ? courses() : this.courses,
      totalCourses: totalCourses ?? this.totalCourses,
      end: end ?? this.end,
    );
  }
}
