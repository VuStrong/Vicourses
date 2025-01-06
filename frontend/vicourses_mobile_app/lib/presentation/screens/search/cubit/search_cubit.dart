import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/course_sort.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'search_state.dart';

class SearchCubit extends Cubit<SearchState> {
  final CourseService _courseService;

  SearchCubit(this._courseService, {int limit = 15})
      : super(SearchState(limit: limit));

  Future<void> search({
    String? searchValue,
  }) async {
    emit(state.copyWith(isLoading: true));

    final result = await _courseService.searchCourses(
      q: searchValue,
      limit: state.limit,
      sort: state.sort,
      rating: state.filter.rating,
      level: state.filter.level,
      free: state.filter.free,
    );

    if (result != null) {
      emit(state.copyWith(
        isLoading: false,
        courses: () => result.items,
        end: result.end,
        total: result.total,
        searchValue: () => searchValue,
      ));
    }
  }

  Future<void> applyFilter() async {

  }

  Future<void> loadMore() async {
    final skip = state.courses != null ? state.courses!.length : 0;

    final result = await _courseService.searchCourses(
      q: state.searchValue,
      skip: skip,
      limit: state.limit,
    );

    if (result != null) {
      state.courses?.addAll(result.items);

      emit(state.copyWith(
        end: result.end,
      ));
    }
  }
}
