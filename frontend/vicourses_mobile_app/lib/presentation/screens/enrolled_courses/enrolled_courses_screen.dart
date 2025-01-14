import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:pull_to_refresh/pull_to_refresh.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/course/learning_course_item.dart';
import 'package:vicourses_mobile_app/presentation/screens/enrolled_courses/cubit/enrolled_courses.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

class EnrolledCoursesScreen extends StatefulWidget {
  const EnrolledCoursesScreen({super.key});

  @override
  State<EnrolledCoursesScreen> createState() => _EnrolledCoursesScreenState();
}

class _EnrolledCoursesScreenState extends State<EnrolledCoursesScreen> {
  late RefreshController _refreshController;

  @override
  void initState() {
    _refreshController = RefreshController();

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    final userId = context.read<UserBloc>().state.user?.id ?? '';

    return BlocProvider<EnrolledCoursesCubit>(
      create: (_) => EnrolledCoursesCubit(CourseService(), userId: userId)
        ..fetchEnrolledCourses(),
      child: Scaffold(
        appBar: AppBar(
          title: Text(AppLocalizations.of(context)!.study),
        ),
        body: MultiBlocListener(
          listeners: [
            // Listener to complete loading more
            BlocListener<EnrolledCoursesCubit, EnrolledCoursesState>(
              listenWhen: (prev, current) =>
                  prev.isLoadingMore && !current.isLoadingMore,
              listener: (context, state) {
                if (state.end) {
                  _refreshController.loadNoData();
                } else {
                  _refreshController.loadComplete();
                }
              },
            ),
            // Listener to complete refreshing
            BlocListener<EnrolledCoursesCubit, EnrolledCoursesState>(
              listenWhen: (prev, current) =>
                  prev.isRefreshing && !current.isRefreshing,
              listener: (context, state) {
                _refreshController.refreshCompleted(resetFooterState: true);
              },
            ),
          ],
          child: BlocBuilder<EnrolledCoursesCubit, EnrolledCoursesState>(
            buildWhen: (prev, current) =>
                prev.isLoading != current.isLoading ||
                prev.courses != current.courses,
            builder: (context, state) {
              if (state.isLoading) {
                return const Center(
                  child: CircularProgressIndicator(),
                );
              }

              return _coursesList(context, state);
            },
          ),
        ),
      ),
    );
  }

  Widget _coursesList(BuildContext context, EnrolledCoursesState state) {
    return SmartRefresher(
      enablePullUp: true,
      enablePullDown: true,
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
        context.read<EnrolledCoursesCubit>().loadMore();
      },
      onRefresh: () {
        context.read<EnrolledCoursesCubit>().refresh();
      },
      child: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 10),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Padding(
              padding: const EdgeInsets.symmetric(vertical: 10),
              child: Text(
                AppLocalizations.of(context)!.enrolledCourses,
                style: const TextStyle(
                  fontSize: 22,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            if (state.courses != null && state.courses!.isNotEmpty)
              ...state.courses!
                  .map((course) => LearningCourseItem(course: course))
                  .toList(),
            if (state.courses == null || state.courses!.isEmpty)
              Text(AppLocalizations.of(context)!.youAreNotEnrolledAnyCourses),
          ],
        ),
      ),
    );
  }
}
