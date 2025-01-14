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
    if (state.isLoading || state.isLoadingMore) return;

    emit(state.copyWith(isLoading: true));

    searchValue ??= state.searchValue;

    final result = await _courseService.searchCourses(
      q: searchValue,
      limit: state.limit,
      sort: state.sort,
      rating: state.rating,
      level: state.level,
      free: state.free,
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

  void applyRatingFilter(num? rating) {
    emit(state.copyWith(rating: () => rating));
  }

  void applyLevelFilter(String? level) {
    emit(state.copyWith(level: () => level));
  }

  void applyFreeFilter(bool? free) {
    emit(state.copyWith(free: () => free));
  }

  void resetFilter() {
    emit(state.copyWith(
      rating: () => null,
      level: () => null,
      free: () => null,
    ));
  }

  void setSort(String sort) {
    emit(state.copyWith(sort: sort));
  }

  Future<void> loadMore() async {
    if (state.isLoading || state.isLoadingMore) return;

    emit(state.copyWith(isLoadingMore: true));

    final skip = state.courses != null ? state.courses!.length : 0;

    final result = await _courseService.searchCourses(
      q: state.searchValue,
      skip: skip,
      limit: state.limit,
      sort: state.sort,
      rating: state.rating,
      level: state.level,
      free: state.free,
    );

    if (result != null) {
      final oldCourses = state.courses ?? [];
      final newCourses = [...oldCourses, ...result.items];

      emit(state.copyWith(
        courses: () => newCourses,
        end: result.end,
        isLoadingMore: false,
      ));
    }
  }
}
