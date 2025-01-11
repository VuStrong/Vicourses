part of 'user_rating_cubit.dart';

enum EditRatingStatus { idle, pending, success, failed }

class UserRatingState {
  final bool isLoading;
  final EditRatingStatus editStatus;
  final Rating? rating;

  UserRatingState({
    this.isLoading = false,
    this.editStatus = EditRatingStatus.idle,
    this.rating,
  });

  UserRatingState copyWith({
    bool? isLoading,
    EditRatingStatus? editStatus,
    Rating? Function()? rating,
  }) {
    return UserRatingState(
      isLoading: isLoading ?? this.isLoading,
      editStatus: editStatus ?? this.editStatus,
      rating: rating != null ? rating() : this.rating,
    );
  }
}
