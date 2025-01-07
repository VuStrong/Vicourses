import 'dart:io';

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:image_picker/image_picker.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/cubit/edit_profile.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class EditThumbnailTab extends StatefulWidget {
  const EditThumbnailTab({super.key});

  @override
  State<EditThumbnailTab> createState() => _EditThumbnailTabState();
}

class _EditThumbnailTabState extends State<EditThumbnailTab>
    with AutomaticKeepAliveClientMixin<EditThumbnailTab> {
  @override
  bool get wantKeepAlive => true;

  final ImagePicker _imagePicker = ImagePicker();

  @override
  Widget build(BuildContext context) {
    super.build(context);

    return Column(
      children: [
        const SizedBox(height: 20),
        _thumbnail(),
        const SizedBox(height: 40),
        _saveButton(),
      ],
    );
  }

  Widget _thumbnail() {
    return BlocBuilder<EditProfileCubit, EditProfileState>(
      buildWhen: (prev, current) => prev.image != current.image,
      builder: (context, state) {
        Widget imageWidget;

        if (state.image != null) {
          imageWidget = Image.file(state.image!, fit: BoxFit.cover);
        } else {
          final thumbnailUrl =
              context.read<UserBloc>().state.user?.thumbnailUrl;

          imageWidget = thumbnailUrl != null
              ? Image.network(thumbnailUrl, fit: BoxFit.cover)
              : Image.asset(AppConstants.defaultUserImagePath);
        }

        return GestureDetector(
          onTap: () {
            _showSelectImageSourceBottomSheet(context);
          },
          child: SizedBox(
            width: 150,
            height: 150,
            child: ClipOval(child: imageWidget),
          ),
        );
      },
    );
  }

  Widget _saveButton() {
    return ElevatedButton(
      onPressed: () {
        context.read<EditProfileCubit>().updateThumbnail();
      },
      style: ElevatedButton.styleFrom(
        backgroundColor: Theme.of(context).colorScheme.primary,
      ),
      child: Text(
        AppLocalizations.of(context)!.save,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }

  Future<void> _onSelectImageSource(
      BuildContext context, ImageSource source) async {
    final XFile? pickedImage = await _imagePicker.pickImage(
      source: source,
      maxHeight: 500,
      maxWidth: 500,
    );

    if (pickedImage != null && context.mounted) {
      context.read<EditProfileCubit>().setImage(File(pickedImage.path));
    }
  }

  void _showSelectImageSourceBottomSheet(BuildContext context) {
    showModalBottomSheet(
      context: context,
      builder: (bottomSheetContext) {
        return Padding(
          padding: const EdgeInsets.only(top: 20, bottom: 20),
          child: Wrap(
            children: [
              ListTile(
                leading: const Icon(Icons.collections),
                title: Text(
                  AppLocalizations.of(context)!.selectImageFromDevice,
                ),
                onTap: () async {
                  Navigator.pop(bottomSheetContext);

                  await _onSelectImageSource(context, ImageSource.gallery);
                },
              ),
              ListTile(
                leading: const Icon(Icons.camera_alt),
                title: Text(AppLocalizations.of(context)!.takeAPhoto),
                onTap: () async {
                  Navigator.pop(bottomSheetContext);

                  await _onSelectImageSource(context, ImageSource.camera);
                },
              ),
            ],
          ),
        );
      },
    );
  }
}
