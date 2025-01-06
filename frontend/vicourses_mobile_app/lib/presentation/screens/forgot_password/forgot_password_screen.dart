import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/error_dialog.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/success_dialog.dart';
import 'package:vicourses_mobile_app/presentation/screens/forgot_password/cubit/forgot_password.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';

class ForgotPasswordScreen extends StatefulWidget {
  ForgotPasswordScreen({super.key});

  final RegExp emailRegex = RegExp(
      r"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.([a-zA-Z]{2,})+");

  @override
  State<ForgotPasswordScreen> createState() => _ForgotPasswordScreenState();
}

class _ForgotPasswordScreenState extends State<ForgotPasswordScreen> {
  final _formKey = GlobalKey<FormState>();

  late final TextEditingController emailController;

  void disposeControllers() {
    emailController.dispose();
  }

  @override
  void initState() {
    emailController = TextEditingController();

    super.initState();
  }

  @override
  void dispose() {
    disposeControllers();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider<ForgotPasswordCubit>(
      create: (_) => ForgotPasswordCubit(AuthService()),
      child: BlocListener<ForgotPasswordCubit, ForgotPasswordState>(
        listener: (context, state) async {
          if (state.status == SendPasswordResetLinkStatus.sending) {
            return context.loaderOverlay.show();
          }

          context.loaderOverlay.hide();

          if (state.status == SendPasswordResetLinkStatus.sendFailed) {
            await showErrorDialog(
              context: context,
              error: state.errorMessage ?? '',
            );
            return;
          }

          if (state.status == SendPasswordResetLinkStatus.sent) {
            await showSuccessDialog(
              context: context,
              text: AppLocalizations.of(context)!.emailHasBeenSent,
            );
          }
        },
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
                        AppLocalizations.of(context)!.forgotPassword,
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
              _buildForm(context),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildForm(BuildContext context) {
    return Form(
      key: _formKey,
      child: Padding(
        padding: const EdgeInsets.all(20),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            Text(AppLocalizations.of(context)!.forgotPasswordInstruction),
            const SizedBox(height: 20),
            _emailField(context),
            const SizedBox(height: 20),
            Row(
              children: [
                Expanded(
                  child: _submitButton(context),
                ),
              ],
            ),
            Row(
              children: [
                Expanded(
                  child: _goToLoginButton(context),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _emailField(BuildContext context) {
    return TextFormField(
      controller: emailController,
      keyboardType: TextInputType.emailAddress,
      textInputAction: TextInputAction.done,
      validator: (value) {
        String msg = AppLocalizations.of(context)!.emailInvalid;

        if (value?.isEmpty ?? true) return msg;

        return widget.emailRegex.hasMatch(value!) ? null : msg;
      },
      decoration: const InputDecoration(
        labelText: 'Email',
        floatingLabelBehavior: FloatingLabelBehavior.always,
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
      ),
    );
  }

  Widget _submitButton(BuildContext context) {
    return BlocBuilder<ForgotPasswordCubit, ForgotPasswordState>(
      builder: (context, state) {
        return ElevatedButton(
          onPressed: () {
            if (_formKey.currentState!.validate()) {
              final email = emailController.text;

              context.read<ForgotPasswordCubit>().sendPasswordResetLink(email);
            }
          },
          style: ElevatedButton.styleFrom(
            backgroundColor: Theme.of(context).colorScheme.primary,
          ),
          child: const Text(
            'OK',
            style: TextStyle(color: Colors.white),
          ),
        );
      },
    );
  }

  Widget _goToLoginButton(BuildContext context) {
    return TextButton(
      onPressed: () {
        context.go(AppRoutes.login);
      },
      child: Text(AppLocalizations.of(context)!.login),
    );
  }
}
