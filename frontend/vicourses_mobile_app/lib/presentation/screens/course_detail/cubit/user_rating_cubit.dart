import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/rating.dart';
import 'package:vicourses_mobile_app/services/api/rating_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'user_rating_state.dart';

class UserRatingCubit extends Cubit<UserRatingState> {
  final RatingService _ratingService;

  UserRatingCubit(this._ratingService) : super(UserRatingState());

  Future<void> fetchUserRating(String courseId) async {
    emit(state.copyWith(isLoading: true));

    final rating = await _ratingService.getUserRating(courseId);

    emit(state.copyWith(
      isLoading: false,
      rating: () => rating,
    ));
  }

  Future<void> editRating({
    required String feedback,
    required int star,
    required String courseId,
  }) async {
    emit(state.copyWith(
      editStatus: EditRatingStatus.pending,
    ));

    try {
      final rating = state.rating != null
          ? await _ratingService.updateRating(
              state.rating!.id,
              feedback: feedback,
              star: star,
            )
          : await _ratingService.createRating(
              courseId: courseId,
              feedback: feedback,
              star: star,
            );

      emit(state.copyWith(
        editStatus: EditRatingStatus.success,
        rating: () => rating,
      ));
    } on AppException {
      emit(state.copyWith(
        editStatus: EditRatingStatus.failed,
      ));
    }
  }

  Future<void> deleteRating() async {
    if (state.rating == null) return;

    await _ratingService.deleteRating(state.rating!.id);

    emit(state.copyWith(
      rating: () => null,
    ));
  }
}
