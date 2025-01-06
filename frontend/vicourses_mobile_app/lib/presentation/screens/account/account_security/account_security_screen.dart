import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

class AccountSecurityScreen extends StatefulWidget {
  const AccountSecurityScreen({super.key});

  @override
  State<AccountSecurityScreen> createState() => _AccountSecurityScreenState();
}

class _AccountSecurityScreenState extends State<AccountSecurityScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.accountSecurity),
      ),
      body: ListView(
        padding: const EdgeInsets.symmetric(horizontal: 10),
        children: [
          const SizedBox(height: 20),
          _emailField(),
          const SizedBox(height: 20),
          _passwordField(),
        ],
      ),
    );
  }

  Widget _emailField() {
    return BlocBuilder<UserBloc, UserState>(
      builder: (context, state) {
        return Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text('Email'),
            ListTile(
              title: Text(state.user?.email ?? ''),
              shape: Border.all(),
            )
          ],
        );
      },
    );
  }

  Widget _passwordField() {
    return BlocBuilder<UserBloc, UserState>(
      builder: (context, state) {
        return Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(AppLocalizations.of(context)!.password),
            ListTile(
              title: const Text('********'),
              shape: Border.all(),
              trailing: IconButton(
                icon: const Icon(Icons.edit),
                onPressed: () {
                  context.push(AppRoutes.changePassword);
                },
              ),
            )
          ],
        );
      },
    );
  }
}
