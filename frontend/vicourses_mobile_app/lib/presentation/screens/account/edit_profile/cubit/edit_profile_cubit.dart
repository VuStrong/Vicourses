import 'dart:io';

import 'package:path/path.dart' as path;
import 'package:uuid/uuid.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/services/api/storage_service.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'edit_profile_state.dart';

class EditProfileCubit extends Cubit<EditProfileState> {
  final UserService _userService;
  final StorageService _storageService;

  EditProfileCubit(this._userService, this._storageService)
      : super(EditProfileState());

  Future<void> editProfile({
    String? name,
    String? headline,
    String? websiteUrl,
    String? youtubeUrl,
    String? facebookUrl,
    String? linkedInUrl,
  }) async {
    emit(state.copyWith(status: EditProfileStatus.pending));

    if (name != null && name.isEmpty) name = null;
    if (headline != null && headline.isEmpty) headline = null;
    if (websiteUrl != null && websiteUrl.isEmpty) websiteUrl = null;
    if (youtubeUrl != null && youtubeUrl.isEmpty) youtubeUrl = null;
    if (facebookUrl != null && facebookUrl.isEmpty) facebookUrl = null;
    if (linkedInUrl != null && linkedInUrl.isEmpty) linkedInUrl = null;

    try {
      final updatedUser = await _userService.editProfile(
        name: name,
        headline: headline,
        websiteUrl: websiteUrl,
        youtubeUrl: youtubeUrl,
        facebookUrl: facebookUrl,
        linkedInUrl: linkedInUrl,
      );

      emit(state.copyWith(
        status: EditProfileStatus.success,
        errorMessage: () => null,
        updatedUser: () => updatedUser,
      ));
    } on AppException catch (e) {
      emit(state.copyWith(
        status: EditProfileStatus.failed,
        errorMessage: () => e.message,
      ));
    }
  }

  Future<void> updateThumbnail() async {
    if (state.image == null) return;

    emit(state.copyWith(status: EditProfileStatus.pending));

    try {
      final ext = path.extension(state.image!.path);
      final uuid = const Uuid().v4();
      final fileId = 'images/vicourses-user-photos/$uuid$ext';

      final uploadResponse = await _storageService.uploadImage(
        state.image!,
        fileId: fileId,
      );

      final updatedUser = await _userService.editProfile(
        thumbnailToken: uploadResponse.token,
      );

      emit(state.copyWith(
        status: EditProfileStatus.success,
        errorMessage: () => null,
        updatedUser: () => updatedUser,
        image: () => null,
      ));
    } on AppException catch (e) {
      emit(state.copyWith(
        status: EditProfileStatus.failed,
        errorMessage: () => e.message,
        image: () => null,
      ));
    }
  }

  void setImage(File image) {
    emit(state.copyWith(image: () => image));
  }
}
