import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
          AppLocalizations.of(context)!.home,
        ),
      ),
      body: ListView(children: [
        ElevatedButton(
          onPressed: () {
            context.read<UserBloc>().add(LogoutUserEvent());
          },
          child: const Text('Logout'),
        ),
        ElevatedButton(
          onPressed: () {
            context.push(AppRoutes.getCourseDetailRoute('abc'));
          },
          child: const Text('Go detail'),
        ),
      ]),
    );
  }
}
