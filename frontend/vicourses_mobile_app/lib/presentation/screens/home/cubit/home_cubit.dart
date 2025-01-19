import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'home_state.dart';

class HomeCubit extends Cubit<HomeState> {
  final CourseService _courseService;

  HomeCubit(this._courseService) : super(HomeState());

  Future<void> fetchHomeContent() async {
    emit(HomeState(isLoading: true));

    final results = await Future.wait([
      _courseService.getCourses(
        limit: 10,
        sort: 'Newest',
      ),
      _courseService.getCourses(
        limit: 10,
        sort: 'HighestRated',
      ),
    ]);

    emit(HomeState(
      isLoading: false,
      newestCourses: results[0]?.items,
      highestRatedCourses: results[1]?.items,
    ));
  }
}
