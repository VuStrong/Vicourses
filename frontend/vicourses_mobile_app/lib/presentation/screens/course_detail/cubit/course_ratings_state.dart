part of 'course_ratings_cubit.dart';

class CourseRatingsState {
  final bool isLoading;
  final bool isLoadingMore;
  final String? courseId;
  final List<Rating> ratings;
  final int totalRating;
  final bool end;

  final int? starFilter;

  CourseRatingsState({
    this.isLoading = false,
    this.isLoadingMore = false,
    this.courseId,
    required this.ratings,
    this.totalRating = 0,
    this.end = true,
    this.starFilter,
  });

  static CourseRatingsState init() {
    return CourseRatingsState(ratings: []);
  }

  CourseRatingsState copyWith({
    bool? isLoading,
    bool? isLoadingMore,
    String? Function()? courseId,
    List<Rating>? ratings,
    int? totalRating,
    bool? end,
    int? Function()? starFilter,
  }) {
    return CourseRatingsState(
      isLoading: isLoading ?? this.isLoading,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      courseId: courseId != null ? courseId() : this.courseId,
      ratings: ratings ?? this.ratings,
      totalRating: totalRating ?? this.totalRating,
      end: end ?? this.end,
      starFilter: starFilter != null ? starFilter() : this.starFilter,
    );
  }
}