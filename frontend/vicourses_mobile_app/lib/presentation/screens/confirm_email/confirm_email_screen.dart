import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/error_dialog.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/success_dialog.dart';
import 'package:vicourses_mobile_app/presentation/screens/confirm_email/cubit/confirm_email.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';

class ConfirmEmailScreen extends StatefulWidget {
  const ConfirmEmailScreen({super.key});

  @override
  State<ConfirmEmailScreen> createState() => _ConfirmEmailScreenState();
}

class _ConfirmEmailScreenState extends State<ConfirmEmailScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider<ConfirmEmailCubit>(
      create: (_) => ConfirmEmailCubit(AuthService()),
      child: Scaffold(
        body: ListView(
          padding: EdgeInsets.zero,
          children: [
            DecoratedBox(
              decoration: const BoxDecoration(
                gradient: LinearGradient(
                  colors: [
                    Color(0xFF007140),
                    Color(0xFF007130),
                    Color(0xFF007120),
                  ],
                ),
              ),
              child: Padding(
                padding: const EdgeInsets.all(20),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    SizedBox(
                      height: MediaQuery.sizeOf(context).height * 0.1,
                    ),
                    Text(
                      AppLocalizations.of(context)!.confirmAccount,
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 34,
                        letterSpacing: 0.5,
                      ),
                    ),
                    const SizedBox(height: 6),
                  ],
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.all(15),
              child: Text(
                AppLocalizations.of(context)!.confirmAccountMessage,
                textAlign: TextAlign.center,
              ),
            ),
            BlocListener<ConfirmEmailCubit, ConfirmEmailState>(
              listener: (
                BuildContext context,
                ConfirmEmailState confirmEmailState,
              ) async {
                if (confirmEmailState.status == ConfirmEmailStatus.sending) {
                  context.loaderOverlay.show();
                  return;
                }

                context.loaderOverlay.hide();

                if (confirmEmailState.status == ConfirmEmailStatus.sendFailed) {
                  await showErrorDialog(
                    context: context,
                    error: confirmEmailState.errorMessage ?? '',
                  );

                  return;
                }

                if (confirmEmailState.status == ConfirmEmailStatus.sent) {
                  await showSuccessDialog(
                    context: context,
                    text: AppLocalizations.of(context)!.emailHasBeenSent,
                  );
                }
              },
              child: Column(
                children: [
                  Padding(
                    padding: const EdgeInsets.all(8),
                    child: _resendButton(context),
                  ),
                  Padding(
                    padding: const EdgeInsets.all(8),
                    child: TextButton(
                      child: Text(AppLocalizations.of(context)!.logout),
                      onPressed: () {
                        context.read<UserBloc>().add(LogoutUserEvent());
                      },
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _resendButton(BuildContext context) {
    return BlocBuilder<UserBloc, UserState>(
      builder: (context, UserState userState) {
        return TextButton(
          child: Text(AppLocalizations.of(context)!.resendEmailConfirmation),
          onPressed: () {
            context
                .read<ConfirmEmailCubit>()
                .resendEmail(userState.user?.email ?? '');
          },
        );
      },
    );
  }
}
