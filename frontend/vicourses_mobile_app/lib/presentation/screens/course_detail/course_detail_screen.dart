import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter_html/flutter_html.dart';
import 'package:share_plus/share_plus.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/bullet_text_list.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/course_detail.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/public_curriculum.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/course_basic_info_section.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/course_purchase_section.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/public_curriculum_section.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

class CourseDetailScreen extends StatefulWidget {
  final String courseId;

  const CourseDetailScreen({
    super.key,
    required this.courseId,
  });

  @override
  State<CourseDetailScreen> createState() => _CourseDetailScreenState();
}

class _CourseDetailScreenState extends State<CourseDetailScreen> {
  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider<CourseDetailCubit>(
          create: (_) =>
              CourseDetailCubit(CourseService())..fetchCourse(widget.courseId),
        ),
        BlocProvider<PublicCurriculumCubit>(
          create: (_) => PublicCurriculumCubit(CourseService()),
        ),
      ],
      child: Scaffold(
        appBar: AppBar(
          title: BlocBuilder<CourseDetailCubit, CourseDetailState>(
            buildWhen: (prev, current) => prev.course != current.course,
            builder: (_, state) => Text(state.course?.title ?? ''),
          ),
          actions: [_shareButton()],
        ),
        body: BlocConsumer<CourseDetailCubit, CourseDetailState>(
          // listener to fetch course resources when course loaded
          listenWhen: (prev, current) =>
              current.course != null && prev.course != current.course,
          listener: (context, state) {
            context
                .read<PublicCurriculumCubit>()
                .fetchPublicCurriculum(state.course!.id);
          },
          buildWhen: (prev, current) =>
              prev.isLoading != current.isLoading ||
              prev.course != current.course,
          builder: (context, state) {
            if (state.isLoading) {
              return const Center(child: CircularProgressIndicator());
            }

            if (state.course == null) {
              return Center(
                child: Text(AppLocalizations.of(context)!.noResults),
              );
            }

            return ListView(
              padding: const EdgeInsets.all(10),
              children: [
                CourseBasicInfoSection(course: state.course!),
                CoursePurchaseSection(course: state.course!),
                _learnContentsSection(context, state),
                const SizedBox(height: 20),
                const PublicCurriculumSection(),
                const SizedBox(height: 30),
                _metricsSection(context, state),
                const SizedBox(height: 20),
                _requirementsSection(context, state),
                const SizedBox(height: 20),
                _targetsSection(context, state),
                const SizedBox(height: 20),
                if (state.course!.description != null)
                  _descriptionSection(context, state),
                const SizedBox(height: 50),
              ],
            );
          },
        ),
      ),
    );
  }

  Widget _learnContentsSection(BuildContext context, CourseDetailState state) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.contents,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        ...state.course!.learnedContents
            .map((content) => ListTile(
                  leading: const Icon(Icons.check),
                  title: Text(content),
                  contentPadding: EdgeInsets.zero,
                ))
            .toList(),
      ],
    );
  }

  Widget _metricsSection(BuildContext context, CourseDetailState state) {
    int hours = (state.course!.metrics.totalVideoDuration / 60 / 60).floor();
    int lessonsCount = state.course!.metrics.lessonsCount;
    int quizzesCount = state.course!.metrics.quizLessonsCount;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.thisCourseContains,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        ListTile(
          leading: const Icon(Icons.ondemand_video_rounded),
          title: Text(AppLocalizations.of(context)!.totalVideoDuration(hours)),
          visualDensity: const VisualDensity(horizontal: 0, vertical: -4),
          contentPadding: EdgeInsets.zero,
        ),
        ListTile(
          leading: const Icon(Icons.play_lesson_rounded),
          title: Text('$lessonsCount ${AppLocalizations.of(context)!.lessons}'),
          visualDensity: const VisualDensity(horizontal: 0, vertical: -4),
          contentPadding: EdgeInsets.zero,
        ),
        ListTile(
          leading: const Icon(Icons.quiz_rounded),
          title: Text('$quizzesCount ${AppLocalizations.of(context)!.quizzes}'),
          visualDensity: const VisualDensity(horizontal: 0, vertical: -4),
          contentPadding: EdgeInsets.zero,
        ),
        ListTile(
          leading: const Icon(Icons.link_rounded),
          title: Text(AppLocalizations.of(context)!.fullLifetimeAccess),
          visualDensity: const VisualDensity(horizontal: 0, vertical: -4),
          contentPadding: EdgeInsets.zero,
        ),
      ],
    );
  }

  Widget _requirementsSection(BuildContext context, CourseDetailState state) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.requirements,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        BulletTextList(texts: state.course!.requirements),
      ],
    );
  }

  Widget _descriptionSection(BuildContext context, CourseDetailState state) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.description,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        Html(data: state.course!.description),
      ],
    );
  }

  Widget _targetsSection(BuildContext context, CourseDetailState state) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.courseTarget,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        BulletTextList(texts: state.course!.targetStudents),
      ],
    );
  }

  Widget _shareButton() {
    return BlocBuilder<CourseDetailCubit, CourseDetailState>(
      buildWhen: (prev, current) => prev.course != current.course,
      builder: (_, state) => IconButton(
        onPressed: () {
          if (state.course == null) return;

          final domain = dotenv.env['WEB_CLIENT_URL'] ?? '';
          final course = state.course!;

          Share.share('$domain/course/${course.titleCleaned}/${course.id}');
        },
        icon: const Icon(Icons.share),
      ),
    );
  }
}
