part of 'home_cubit.dart';

class HomeState {
  final bool isLoading;
  final List<Course>? courses;

  HomeState({
    this.isLoading = false,
    this.courses,
  });
}
