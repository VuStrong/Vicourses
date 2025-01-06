import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/edit_basic_info_tab.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/edit_profile/edit_thumbnail_tab.dart';

class EditProfileScreen extends StatefulWidget {
  const EditProfileScreen({super.key});

  @override
  State<EditProfileScreen> createState() => _EditProfileScreenState();
}

class _EditProfileScreenState extends State<EditProfileScreen> {
  @override
  Widget build(BuildContext context) {
    final user = context.read<UserBloc>().state.user;

    return DefaultTabController(
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
        body: const TabBarView(
          children: [
            EditBasicInfoTab(),
            EditThumbnailTab(),
          ],
        ),
      ),
    );
  }
}
