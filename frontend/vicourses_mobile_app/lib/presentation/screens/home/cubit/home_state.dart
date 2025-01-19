part of 'home_cubit.dart';

class HomeState {
  final bool isLoading;
  final List<Course>? newestCourses;
  final List<Course>? highestRatedCourses;

  HomeState({
    this.isLoading = false,
    this.newestCourses,
    this.highestRatedCourses,
  });
}
