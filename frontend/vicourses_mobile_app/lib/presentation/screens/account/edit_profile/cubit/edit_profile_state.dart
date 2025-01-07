part of 'edit_profile_cubit.dart';

enum EditProfileStatus { initial, pending, success, failed }

class EditProfileState {
  final EditProfileStatus status;
  final File? image;
  final String? errorMessage;
  final User? updatedUser;

  EditProfileState({
    this.status = EditProfileStatus.initial,
    this.image,
    this.errorMessage,
    this.updatedUser,
  });

  EditProfileState copyWith({
    EditProfileStatus? status,
    File? Function()? image,
    String? Function()? errorMessage,
    User? Function()? updatedUser,
  }) {
    return EditProfileState(
      status: status ?? this.status,
      image: image?.call() ?? this.image,
      errorMessage: errorMessage?.call() ?? this.errorMessage,
      updatedUser: updatedUser?.call() ?? this.updatedUser,
    );
  }
}
