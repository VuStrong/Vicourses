import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/prompt_dialog.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class AccountOverviewScreen extends StatefulWidget {
  const AccountOverviewScreen({super.key});

  @override
  State<AccountOverviewScreen> createState() => _AccountOverviewScreenState();
}

class _AccountOverviewScreenState extends State<AccountOverviewScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.account),
      ),
      body: ListView(
        children: [
          const SizedBox(height: 20),
          _accountThumb(),
          const SizedBox(height: 20),
          _goToProfileScreenButton(context),
          const SizedBox(height: 20),
          Padding(
            padding: const EdgeInsets.only(left: 16),
            child: Text(
              AppLocalizations.of(context)!.accountSettings,
              style: const TextStyle(fontSize: 13),
            ),
          ),
          ListTile(
            title: Text(AppLocalizations.of(context)!.editAccountProfile),
            trailing: const Icon(Icons.arrow_forward_ios),
            onTap: () {
              context.push(AppRoutes.editProfile);
            },
          ),
          ListTile(
            title: Text(AppLocalizations.of(context)!.accountSecurity),
            trailing: const Icon(Icons.arrow_forward_ios),
            onTap: () {
              context.push(AppRoutes.accountSecurity);
            },
          ),
          _logoutButton(context),
        ],
      ),
    );
  }

  Widget _accountThumb() {
    return BlocBuilder<UserBloc, UserState>(
      builder: (context, state) {
        if (state.user == null) {
          return const SizedBox();
        }

        return Column(
          children: [
            SizedBox(
              width: 150,
              height: 150,
              child: ClipOval(
                child: state.user!.thumbnailUrl != null
                    ? Image.network(
                        state.user!.thumbnailUrl!,
                        fit: BoxFit.cover,
                      )
                    : Image.asset(
                        AppConstants.defaultUserImagePath,
                        fit: BoxFit.cover,
                      ),
              ),
            ),
            const SizedBox(height: 20),
            Text(
              state.user!.name,
              style: const TextStyle(fontSize: 20),
            ),
            Text(
              state.user!.email,
              style: const TextStyle(
                fontSize: 16,
                color: Colors.grey,
              ),
            ),
          ],
        );
      },
    );
  }

  Widget _goToProfileScreenButton(BuildContext context) {
    return BlocBuilder<UserBloc, UserState>(
      buildWhen: (prev, current) => prev.user?.id != current.user?.id,
      builder: (context, state) {
        if (state.user == null) {
          return const SizedBox();
        }

        return TextButton(
          onPressed: () async {
            context.push(AppRoutes.getUserProfileRoute(state.user!.id));
          },
          child: Text(
            AppLocalizations.of(context)!.seeYourPublicProfile,
            style: const TextStyle(fontSize: 18),
          ),
        );
      },
    );
  }

  Widget _logoutButton(BuildContext context) {
    final logoutStr = AppLocalizations.of(context)!.logout;

    return TextButton(
      onPressed: () async {
        final shouldLogout = await showPromptDialog(
          context: context,
          prompt: AppLocalizations.of(context)!.logoutConfirm,
          title: logoutStr,
        );

        if (shouldLogout && context.mounted) {
          context.read<UserBloc>().add(LogoutUserEvent());
        }
      },
      child: Text(
        logoutStr,
        style: const TextStyle(fontSize: 18),
      ),
    );
  }
}
