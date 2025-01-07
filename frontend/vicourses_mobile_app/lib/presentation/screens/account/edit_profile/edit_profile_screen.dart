import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/snack_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/cubit/edit_profile.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/edit_basic_info_tab.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/edit_thumbnail_tab.dart';
import 'package:vicourses_mobile_app/services/api/storage_service.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';

class EditProfileScreen extends StatefulWidget {
  const EditProfileScreen({super.key});

  @override
  State<EditProfileScreen> createState() => _EditProfileScreenState();
}

class _EditProfileScreenState extends State<EditProfileScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider<EditProfileCubit>(
      create: (_) => EditProfileCubit(UserService(), StorageService()),
      child: BlocListener<EditProfileCubit, EditProfileState>(
        listenWhen: (prev, current) => prev.status != current.status,
        listener: (context, state) {
          if (state.status == EditProfileStatus.pending) {
            return context.loaderOverlay.show();
          }

          context.loaderOverlay.hide();

          if (state.status == EditProfileStatus.failed) {
            return showSnackBar(
              context: context,
              text: AppLocalizations.of(context)!.errorOccurred,
              type: SnackBarType.error,
            );
          }

          if (state.status == EditProfileStatus.success) {
            context
                .read<UserBloc>()
                .add(UserUpdatedEvent(user: state.updatedUser!));

            showSnackBar(
              context: context,
              text: AppLocalizations.of(context)!.saved,
              type: SnackBarType.success,
            );
          }
        },
        child: DefaultTabController(
          length: 2,
          child: Scaffold(
            appBar: AppBar(
              title: Text(AppLocalizations.of(context)!.editAccountProfile),
              bottom: TabBar(
                tabs: [
                  Tab(
                    text: AppLocalizations.of(context)!.basicInfo,
                  ),
                  Tab(
                    text: AppLocalizations.of(context)!.thumbnail,
                  ),
                ],
              ),
            ),
            body: TabBarView(
              children: [
                EditBasicInfoTab(),
                const EditThumbnailTab(),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
