import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:go_router/go_router.dart';
import 'package:google_sign_in/google_sign_in.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/error_dialog.dart';
import 'package:vicourses_mobile_app/presentation/screens/login/cubit/login.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';

class LoginScreen extends StatefulWidget {
  LoginScreen({super.key});

  final RegExp emailRegex = RegExp(
      r"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.([a-zA-Z]{2,})+");

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();

  late final TextEditingController emailController;
  late final TextEditingController passwordController;
  final GoogleSignIn _googleSignIn = GoogleSignIn(
    clientId: dotenv.env['GOOGLE_CLIENT_ID'],
  );

  void disposeControllers() {
    emailController.dispose();
    passwordController.dispose();
  }

  @override
  void initState() {
    emailController = TextEditingController();
    passwordController = TextEditingController();

    super.initState();
  }

  @override
  void dispose() {
    disposeControllers();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider<LoginCubit>(
      create: (_) => LoginCubit(AuthService()),
      child: BlocListener<LoginCubit, LoginState>(
        listenWhen: (prev, current) => prev.status != current.status,
        listener: (context, state) async {
          if (state.status == LoginStatus.pending) {
            return context.loaderOverlay.show();
          }

          context.loaderOverlay.hide();

          if (state.status == LoginStatus.failed) {
            await showErrorDialog(
              context: context,
              error: state.errorMessage ?? '',
            );
            return;
          }

          if (state.status == LoginStatus.success) {
            context
                .read<UserBloc>()
                .add(LoginUserEvent(loginResponse: state.loginResponse!));
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
                        AppLocalizations.of(context)!.loginHeading,
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
            _emailField(context),
            _passwordField(),
            _forgotPasswordButton(context),
            const SizedBox(height: 20),
            Row(
              children: [
                Expanded(
                  child: _submitButton(),
                ),
              ],
            ),
            const SizedBox(height: 20),
            Row(
              children: [
                Expanded(child: Divider(color: Colors.grey.shade200)),
                Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 20),
                  child: Text(
                    AppLocalizations.of(context)!.orLoginWith,
                    style: const TextStyle(
                      letterSpacing: 0.5,
                    ),
                  ),
                ),
                Expanded(child: Divider(color: Colors.grey.shade200)),
              ],
            ),
            const SizedBox(height: 20),
            Row(
              children: [
                Expanded(
                  child: _loginWithGoogleButton(context),
                ),
              ],
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  '${AppLocalizations.of(context)!.notHaveAccount}?',
                  style: const TextStyle(
                    letterSpacing: 0.5,
                  ),
                ),
                const SizedBox(width: 4),
                TextButton(
                  onPressed: () {
                    context.go(AppRoutes.register);
                  },
                  child: Text(AppLocalizations.of(context)!.register),
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
      textInputAction: TextInputAction.next,
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

  Widget _passwordField() {
    return BlocBuilder<LoginCubit, LoginState>(
      buildWhen: (prev, cur) => prev.passwordObscure != cur.passwordObscure,
      builder: (context, state) {
        return TextFormField(
          controller: passwordController,
          keyboardType: TextInputType.visiblePassword,
          textInputAction: TextInputAction.done,
          validator: (value) {
            if (value?.isEmpty ?? true) {
              return AppLocalizations.of(context)!.requiredField;
            }

            return null;
          },
          obscureText: state.passwordObscure,
          decoration: InputDecoration(
            suffixIcon: IconButton(
              onPressed: () {
                context.read<LoginCubit>().togglePasswordObscure();
              },
              style: IconButton.styleFrom(
                minimumSize: const Size.square(48),
              ),
              icon: Icon(
                state.passwordObscure
                    ? Icons.visibility_off_outlined
                    : Icons.visibility_outlined,
                size: 20,
              ),
            ),
            labelText: AppLocalizations.of(context)!.password,
            floatingLabelBehavior: FloatingLabelBehavior.always,
          ),
          onTapOutside: (event) => FocusScope.of(context).unfocus(),
          style: const TextStyle(
            fontWeight: FontWeight.w500,
          ),
        );
      },
    );
  }

  Widget _forgotPasswordButton(BuildContext context) {
    return TextButton(
      onPressed: () {
        context.push(AppRoutes.forgotPassword);
      },
      child: Text(
        '${AppLocalizations.of(context)!.forgotPassword}?',
      ),
    );
  }

  Widget _submitButton() {
    return BlocBuilder<LoginCubit, LoginState>(
      builder: (context, state) {
        return ElevatedButton(
          onPressed: () {
            if (_formKey.currentState!.validate()) {
              final email = emailController.text;
              final password = passwordController.text;

              context.read<LoginCubit>().login(email, password);
            }
          },
          style: ElevatedButton.styleFrom(
            backgroundColor: Theme.of(context).colorScheme.primary,
          ),
          child: Text(
            AppLocalizations.of(context)!.login,
            style: const TextStyle(color: Colors.white),
          ),
        );
      },
    );
  }

  Widget _loginWithGoogleButton(BuildContext context) {
    return BlocBuilder<LoginCubit, LoginState>(
      builder: (context, _) {
        return OutlinedButton.icon(
          onPressed: () async {
            try {
              final account = await _googleSignIn.signIn();
              final authentication = await account?.authentication;

              if (authentication?.idToken != null && context.mounted) {
                context
                    .read<LoginCubit>()
                    .loginWithGoogle(authentication!.idToken!);
              }
            } catch (e) {
              //
            }
          },
          icon: SvgPicture.asset(
            'assets/svg/google.svg',
            width: 14,
          ),
          label: const Text(
            'Google',
            style: TextStyle(color: Colors.black),
          ),
        );
      },
    );
  }
}
