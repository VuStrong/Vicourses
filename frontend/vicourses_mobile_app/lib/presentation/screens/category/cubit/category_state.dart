part of 'category_cubit.dart';

class CategoryState {
  final Category? category;
  final bool isLoading;
  final bool isLoadingMore;
  final List<Course>? courses;
  final int total;
  final bool end;
  final String sort;

  final num? rating;
  final String? level;
  final bool? free;

  CategoryState({
    this.category,
    this.isLoading = false,
    this.isLoadingMore = false,
    this.courses,
    this.total = 0,
    this.end = true,
    this.sort = CourseSort.newest,
    this.rating,
    this.free,
    this.level,
  });

  CategoryState copyWith({
    bool? isLoading,
    bool? isLoadingMore,
    int? total,
    bool? end,
    List<Course>? Function()? courses,
    String? sort,
    num? Function()? rating,
    String? Function()? level,
    bool? Function()? free,
  }) {
    return CategoryState(
      category: category,
      isLoading: isLoading ?? this.isLoading,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      total: total ?? this.total,
      end: end ?? this.end,
      courses: courses != null ? courses() : this.courses,
      sort: sort ?? this.sort,
      rating: rating != null ? rating() : this.rating,
      level: level != null ? level() : this.level,
      free: free != null ? free() : this.free,
    );
  }
}
