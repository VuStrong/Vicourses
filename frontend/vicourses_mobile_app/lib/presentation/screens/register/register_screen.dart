import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:go_router/go_router.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/inputs/text_input.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/error_dialog.dart';
import 'package:vicourses_mobile_app/presentation/screens/register/cubit/register.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/services/api/auth_service.dart';

class RegisterScreen extends StatefulWidget {
  RegisterScreen({super.key});

  final RegExp emailRegex = RegExp(
      r"^[a-zA-Z0-9.a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9]+\.([a-zA-Z]{2,})+");

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _formKey = GlobalKey<FormState>();

  final ValueNotifier<bool> passwordNotifier = ValueNotifier(true);

  late final TextEditingController nameController;
  late final TextEditingController emailController;
  late final TextEditingController passwordController;

  void disposeControllers() {
    nameController.dispose();
    emailController.dispose();
    passwordController.dispose();
  }

  @override
  void initState() {
    nameController = TextEditingController();
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
    return BlocProvider<RegisterCubit>(
        create: (_) => RegisterCubit(AuthService()),
        child: BlocListener<RegisterCubit, RegisterState>(
          listener: (context, state) async {
            if (state.status == RegisterStatus.pending) {
              context.loaderOverlay.show();

              return;
            }

            context.loaderOverlay.hide();

            if (state.status == RegisterStatus.failed) {
              await showErrorDialog(
                context: context,
                error: state.errorMessage ?? '',
              );
              return;
            }

            context
                .read<UserBloc>()
                .add(LoginUserEvent(loginResponse: state.loginResponse!));
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
                          AppLocalizations.of(context)!.register,
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
        ));
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
            _nameField(context),
            _emailField(context),
            _passwordField(context),
            const SizedBox(height: 20),
            Row(
              children: [
                Expanded(
                  child: _submitButton(context),
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
                  '${AppLocalizations.of(context)!.alreadyHaveAccount}?',
                  style: const TextStyle(letterSpacing: 0.5),
                ),
                TextButton(
                  onPressed: () {
                    context.go(AppRoutes.login);
                  },
                  child: Text(AppLocalizations.of(context)!.login),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _nameField(BuildContext context) {
    return TextInput(
      controller: nameController,
      labelText: AppLocalizations.of(context)!.username,
      keyboardType: TextInputType.text,
      textInputAction: TextInputAction.next,
      validator: (value) {
        String msg = AppLocalizations.of(context)!.usernameLength;

        return value!.isEmpty
            ? msg
            : value.length >= 2 && value.length <= 50
            ? null
            : msg;
      },
    );
  }

  Widget _emailField(BuildContext context) {
    return TextInput(
      controller: emailController,
      labelText: 'Email',
      keyboardType: TextInputType.emailAddress,
      textInputAction: TextInputAction.next,
      validator: (value) {
        String msg = AppLocalizations.of(context)!.emailInvalid;

        return value!.isEmpty
            ? msg
            : widget.emailRegex.hasMatch(value)
                ? null
                : msg;
      },
    );
  }

  Widget _passwordField(BuildContext context) {
    return ValueListenableBuilder(
      valueListenable: passwordNotifier,
      builder: (_, passwordObscure, __) {
        return TextInput(
          obscureText: passwordObscure,
          controller: passwordController,
          labelText: AppLocalizations.of(context)!.password,
          textInputAction: TextInputAction.done,
          keyboardType: TextInputType.visiblePassword,
          validator: (value) {
            String msg = AppLocalizations.of(context)!.passwordLength;

            return value!.isEmpty
                ? msg
                : value.length >= 8 && value.length <= 50
                ? null
                : msg;
          },
          suffixIcon: IconButton(
            onPressed: () => passwordNotifier.value = !passwordObscure,
            style: IconButton.styleFrom(
              minimumSize: const Size.square(48),
            ),
            icon: Icon(
              passwordObscure
                  ? Icons.visibility_off_outlined
                  : Icons.visibility_outlined,
              size: 20,
            ),
          ),
        );
      },
    );
  }

  Widget _submitButton(BuildContext context) {
    return BlocBuilder<RegisterCubit, RegisterState>(
      builder: (context, state) {
        return ElevatedButton(
          onPressed: () {
            if (_formKey.currentState!.validate()) {
              final name = nameController.text;
              final email = emailController.text;
              final password = passwordController.text;

              context
                  .read<RegisterCubit>()
                  .register(name: name, email: email, password: password);
            }
          },
          style: ElevatedButton.styleFrom(
            backgroundColor: Theme.of(context).colorScheme.primary,
          ),
          child: Text(
            AppLocalizations.of(context)!.register,
            style: const TextStyle(color: Colors.white),
          ),
        );
      },
    );
  }

  Widget _loginWithGoogleButton(BuildContext context) {
    return OutlinedButton.icon(
      onPressed: () {
        //
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
  }
}
