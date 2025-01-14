import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/rating.dart';
import 'package:vicourses_mobile_app/services/api/rating_service.dart';

part 'course_ratings_state.dart';

class CourseRatingsCubit extends Cubit<CourseRatingsState> {
  final int limit;
  final RatingService _ratingService;

  CourseRatingsCubit(this._ratingService, {this.limit = 10})
      : super(CourseRatingsState.init());

  Future<void> fetchRatings(String courseId, {int? starFilter}) async {
    if (state.isLoading) return;

    emit(state.copyWith(
      isLoading: true,
      courseId: () => courseId,
      starFilter: () => starFilter,
    ));

    final result = await _ratingService.getCourseRatings(
      courseId,
      limit: limit,
      star: starFilter,
    );

    emit(state.copyWith(
      isLoading: false,
      ratings: result?.items ?? [],
      totalRating: result?.total ?? 0,
      end: result?.end ?? true,
    ));
  }

  Future<void> loadMore() async {
    if (state.courseId == null || state.isLoading || state.isLoadingMore) return;

    emit(state.copyWith(isLoadingMore: true));

    final skip = state.ratings.length;

    final result = await _ratingService.getCourseRatings(
      state.courseId!,
      skip: skip,
      limit: limit,
      star: state.starFilter,
    );

    if (result != null) {
      emit(state.copyWith(
        isLoadingMore: false,
        ratings: [...state.ratings, ...result.items],
        end: result.end,
      ));
      return;
    }

    emit(state.copyWith(isLoadingMore: false));
  }
}
