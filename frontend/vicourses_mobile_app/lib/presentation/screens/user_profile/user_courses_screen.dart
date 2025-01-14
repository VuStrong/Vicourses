import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:pull_to_refresh/pull_to_refresh.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/course/course_item.dart';
import 'package:vicourses_mobile_app/presentation/screens/user_profile/cubit/user_courses.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

class UserCoursesScreen extends StatefulWidget {
  final String userId;

  const UserCoursesScreen({
    super.key,
    required this.userId,
  });

  @override
  State<UserCoursesScreen> createState() => _UserCoursesScreenState();
}

class _UserCoursesScreenState extends State<UserCoursesScreen> {
  late RefreshController _refreshController;

  @override
  Widget build(BuildContext context) {
    _refreshController = RefreshController();

    return BlocProvider<UserCoursesCubit>(
      create: (_) => UserCoursesCubit(CourseService(), userId: widget.userId)
        ..fetchCourses(),
      child: Scaffold(
        appBar: AppBar(
          title: Text(AppLocalizations.of(context)!.courses),
        ),
        body: BlocConsumer<UserCoursesCubit, UserCoursesState>(
          listener: (context, state) {
            if (state.isLoading) {
              return;
            }

            if (state.end) {
              _refreshController.loadNoData();
            } else {
              _refreshController.loadComplete();
            }
          },
          builder: (context, state) {
            if (state.isLoading) {
              return const Center(
                child: CircularProgressIndicator(),
              );
            }

            if (state.courses == null || state.courses!.isEmpty) {
              return Center(
                child: Text(AppLocalizations.of(context)!.noResults),
              );
            }

            return _coursesList(context, state);
          },
        ),
      ),
    );
  }

  Widget _coursesList(BuildContext context, UserCoursesState state) {
    return SmartRefresher(
      enablePullUp: true,
      enablePullDown: false,
      footer: CustomFooter(
        builder: (BuildContext context, LoadStatus? mode) {
          late final Widget body;

          if (mode == LoadStatus.loading) {
            body = const CircularProgressIndicator();
          } else {
            body = const SizedBox.shrink();
          }

          return SizedBox(
            height: 55,
            child: Center(child: body),
          );
        },
      ),
      controller: _refreshController,
      onLoading: () {
        context.read<UserCoursesCubit>().loadMore();
      },
      child: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 10),
        child: Column(
          children: state.courses!
              .map((course) => CourseItem(course: course))
              .toList(),
        ),
      ),
    );
  }
}
