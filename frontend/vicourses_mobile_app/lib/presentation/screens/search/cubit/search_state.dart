part of 'search_cubit.dart';

class SearchState {
  final bool isLoading;
  final List<Course>? courses;
  final String? searchValue;
  final int limit;
  final int total;
  final bool end;
  late final SearchFilter filter;
  final String sort;

  SearchState({
    this.isLoading = false,
    this.courses,
    this.searchValue,
    this.limit = 15,
    this.total = 0,
    this.end = true,
    this.sort = CourseSort.relevance,
  }) {
    filter = SearchFilter();
  }

  SearchState copyWith({
    bool? isLoading,
    int? limit,
    int? total,
    bool? end,
    List<Course>? Function()? courses,
    String? Function()? searchValue,
    String? sort,
  }) {
    return SearchState(
      isLoading: isLoading ?? this.isLoading,
      limit: limit ?? this.limit,
      total: total ?? this.total,
      end: end ?? this.end,
      courses: courses != null ? courses() : this.courses,
      searchValue: searchValue != null ? searchValue() : this.searchValue,
      sort: sort ?? this.sort,
    );
  }
}

class SearchFilter {
  final String? level;
  final bool? free;
  final num? rating;

  SearchFilter({
    this.level,
    this.free,
    this.rating,
  });
}
