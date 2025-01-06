import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
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
          listenWhen: (prev, current) => prev.status != current.status,
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

            if (state.status == RegisterStatus.success) {
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
    return TextFormField(
      controller: nameController,
      keyboardType: TextInputType.text,
      textInputAction: TextInputAction.next,
      validator: (value) {
        String msg = AppLocalizations.of(context)!.textLength(2, 50);

        if (value?.isEmpty ?? true) return msg;

        return value!.length >= 2 && value.length <= 50 ? null : msg;
      },
      decoration: InputDecoration(
        labelText: AppLocalizations.of(context)!.username,
        floatingLabelBehavior: FloatingLabelBehavior.always,
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      style: const TextStyle(
        fontWeight: FontWeight.w500,
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

  Widget _passwordField(BuildContext context) {
    return BlocBuilder<RegisterCubit, RegisterState>(
      buildWhen: (prev, cur) => prev.passwordObscure != cur.passwordObscure,
      builder: (context, state) {
        return TextFormField(
          controller: passwordController,
          keyboardType: TextInputType.visiblePassword,
          textInputAction: TextInputAction.done,
          validator: (value) {
            String msg = AppLocalizations.of(context)!.textLength(8, 50);

            if (value?.isEmpty ?? true) return msg;

            return value!.length >= 8 && value.length <= 50 ? null : msg;
          },
          obscureText: state.passwordObscure,
          decoration: InputDecoration(
            suffixIcon: IconButton(
              onPressed: () {
                context.read<RegisterCubit>().togglePasswordObscure();
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
}
