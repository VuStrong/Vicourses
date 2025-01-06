import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/error_dialog.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/dialogs/success_dialog.dart';
import 'package:vicourses_mobile_app/presentation/screens/account/account_security/cubit/change_password.dart';
import 'package:vicourses_mobile_app/services/api/user_service.dart';

class ChangePasswordScreen extends StatefulWidget {
  const ChangePasswordScreen({super.key});

  @override
  State<ChangePasswordScreen> createState() => _ChangePasswordScreenState();
}

class _ChangePasswordScreenState extends State<ChangePasswordScreen> {
  final _formKey = GlobalKey<FormState>();
  late final TextEditingController oldPasswordController;
  late final TextEditingController newPasswordController;

  @override
  void initState() {
    oldPasswordController = TextEditingController();
    newPasswordController = TextEditingController();

    super.initState();
  }

  @override
  void dispose() {
    oldPasswordController.dispose();
    newPasswordController.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider<ChangePasswordCubit>(
      create: (_) => ChangePasswordCubit(UserService()),
      child: Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          title: Text(AppLocalizations.of(context)!.changePassword),
          actions: [
            IconButton(
              onPressed: () {
                context.pop();
              },
              icon: const Icon(Icons.close),
            ),
          ],
        ),
        body: BlocListener<ChangePasswordCubit, ChangePasswordState>(
          listenWhen: (prev, current) => prev.status != current.status,
          listener: (context, state) async {
            if (state.status == ChangePasswordStatus.pending) {
              return context.loaderOverlay.show();
            }

            context.loaderOverlay.hide();

            if (state.status == ChangePasswordStatus.failed) {
              await showErrorDialog(
                context: context,
                error: state.errorMessage ?? '',
              );
              return;
            }

            if (state.status == ChangePasswordStatus.success) {
              oldPasswordController.text = '';
              newPasswordController.text = '';

              await showSuccessDialog(
                context: context,
                text: AppLocalizations.of(context)!.passwordChanged,
              );
            }
          },
          child: _buildForm(context),
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
          children: [
            _oldPasswordField(),
            _newPasswordField(),
            const SizedBox(height: 20),
            Row(
              children: [
                Expanded(
                  child: _submitButton(context),
                ),
              ],
            ),
          ],
        ),
      ),
    );
  }

  Widget _oldPasswordField() {
    return BlocBuilder<ChangePasswordCubit, ChangePasswordState>(
      buildWhen: (prev, cur) =>
          prev.oldPasswordObscure != cur.oldPasswordObscure,
      builder: (context, state) {
        return TextFormField(
          controller: oldPasswordController,
          keyboardType: TextInputType.visiblePassword,
          textInputAction: TextInputAction.next,
          validator: (value) {
            if (value?.isEmpty ?? true) {
              return AppLocalizations.of(context)!.requiredField;
            }

            return null;
          },
          obscureText: state.oldPasswordObscure,
          decoration: InputDecoration(
            suffixIcon: IconButton(
              onPressed: () {
                context.read<ChangePasswordCubit>().toggleOldPasswordObscure();
              },
              style: IconButton.styleFrom(
                minimumSize: const Size.square(48),
              ),
              icon: Icon(
                state.oldPasswordObscure
                    ? Icons.visibility_off_outlined
                    : Icons.visibility_outlined,
                size: 20,
              ),
            ),
            labelText: AppLocalizations.of(context)!.oldPassword,
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

  Widget _newPasswordField() {
    return BlocBuilder<ChangePasswordCubit, ChangePasswordState>(
      buildWhen: (prev, cur) =>
          prev.newPasswordObscure != cur.newPasswordObscure,
      builder: (context, state) {
        return TextFormField(
          controller: newPasswordController,
          keyboardType: TextInputType.visiblePassword,
          textInputAction: TextInputAction.done,
          validator: (value) {
            final msg = AppLocalizations.of(context)!.textLength(8, 50);

            if (value?.isEmpty ?? true) return msg;

            return value!.length >= 8 && value.length <= 50 ? null : msg;
          },
          obscureText: state.newPasswordObscure,
          decoration: InputDecoration(
            suffixIcon: IconButton(
              onPressed: () {
                context.read<ChangePasswordCubit>().toggleNewPasswordObscure();
              },
              style: IconButton.styleFrom(
                minimumSize: const Size.square(48),
              ),
              icon: Icon(
                state.newPasswordObscure
                    ? Icons.visibility_off_outlined
                    : Icons.visibility_outlined,
                size: 20,
              ),
            ),
            labelText: AppLocalizations.of(context)!.newPassword,
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
    return BlocBuilder<ChangePasswordCubit, ChangePasswordState>(
      builder: (context, state) {
        return ElevatedButton(
          onPressed: () {
            if (_formKey.currentState!.validate()) {
              final oldPassword = oldPasswordController.text;
              final newPassword = newPasswordController.text;

              context.read<ChangePasswordCubit>().changePassword(
                    oldPassword: oldPassword,
                    newPassword: newPassword,
                  );
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
}
