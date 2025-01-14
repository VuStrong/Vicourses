part of 'search_cubit.dart';

class SearchState {
  final bool isLoading;
  final bool isLoadingMore;
  final List<Course>? courses;
  final String? searchValue;
  final int limit;
  final int total;
  final bool end;
  final String sort;

  final num? rating;
  final String? level;
  final bool? free;

  SearchState({
    this.isLoading = false,
    this.isLoadingMore = false,
    this.courses,
    this.searchValue,
    this.limit = 15,
    this.total = 0,
    this.end = true,
    this.sort = CourseSort.relevance,
    this.rating,
    this.free,
    this.level,
  });

  SearchState copyWith({
    bool? isLoading,
    bool? isLoadingMore,
    int? limit,
    int? total,
    bool? end,
    List<Course>? Function()? courses,
    String? Function()? searchValue,
    String? sort,
    num? Function()? rating,
    String? Function()? level,
    bool? Function()? free,
  }) {
    return SearchState(
      isLoading: isLoading ?? this.isLoading,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      limit: limit ?? this.limit,
      total: total ?? this.total,
      end: end ?? this.end,
      courses: courses != null ? courses() : this.courses,
      searchValue: searchValue != null ? searchValue() : this.searchValue,
      sort: sort ?? this.sort,
      rating: rating != null ? rating() : this.rating,
      level: level != null ? level() : this.level,
      free: free != null ? free() : this.free,
    );
  }
}
