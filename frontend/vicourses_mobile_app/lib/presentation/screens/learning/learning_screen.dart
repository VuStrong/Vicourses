import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/lesson.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/hls_video_player.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/comments_tab.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/comments.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/learning.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/curriculum_tab.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/doing_quizzes_screen.dart';
import 'package:vicourses_mobile_app/services/api/comment_service.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class LearningScreen extends StatefulWidget {
  final String courseId;

  const LearningScreen({
    super.key,
    required this.courseId,
  });

  @override
  State<LearningScreen> createState() => _LearningScreenState();
}

class _LearningScreenState extends State<LearningScreen> {
  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider<LearningCubit>(
          create: (_) =>
              LearningCubit(CourseService())..fetchCurriculum(widget.courseId),
        ),
        BlocProvider<CommentsCubit>(
          create: (_) => CommentsCubit(CommentService()),
        ),
      ],
      child: BlocListener<LearningCubit, LearningState>(
        listenWhen: (prev, current) =>
            current.currentLesson != null &&
            prev.currentLesson?.id != current.currentLesson!.id,
        listener: (context, state) {
          context.read<CommentsCubit>().setLessonId(state.currentLesson!.id);
        },
        child: Scaffold(
          body: Column(
            children: [
              _lessonBuilder(),
              _tabView(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _lessonBuilder() {
    return AspectRatio(
      aspectRatio: 16 / 9,
      child: BlocBuilder<LearningCubit, LearningState>(
        buildWhen: (prev, current) =>
            prev.isLoadingCurrentLesson != current.isLoadingCurrentLesson ||
            prev.currentLesson != current.currentLesson,
        builder: (context, state) {
          if (state.isLoadingCurrentLesson) {
            return const Center(child: CircularProgressIndicator());
          }

          if (state.currentLesson == null) {
            return Center(
              child: Text(AppLocalizations.of(context)!.noResults),
            );
          }

          return state.currentLesson!.type == LessonType.video
              ? _videoLesson(context, state.currentLesson!)
              : _quizLesson(context, state.currentLesson!);
        },
      ),
    );
  }

  Widget _videoLesson(BuildContext context, Lesson lesson) {
    if (lesson.video == null || lesson.video!.status != VideoStatus.processed) {
      return Center(
        child: Text(AppLocalizations.of(context)!.noResults),
      );
    }

    return HlsVideoPlayer(token: lesson.video!.token);
  }

  Widget _quizLesson(BuildContext context, Lesson lesson) {
    final localizations = AppLocalizations.of(context)!;

    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        Text(
          '${lesson.quizzesCount} ${localizations.quizzes}',
          style: const TextStyle(
            fontWeight: FontWeight.bold,
          ),
        ),
        OutlinedButton(
          onPressed: () {
            Navigator.of(context).push(MaterialPageRoute(
              builder: (_) => DoingQuizzesScreen(
                quizzes: lesson.quizzes,
              ),
            ));
          },
          child: Text(localizations.startDoingQuizzes),
        ),
      ],
    );
  }

  Widget _tabView() {
    return DefaultTabController(
      length: 2,
      child: Expanded(
        child: Column(
          children: [
            TabBar(
              tabs: [
                Tab(text: AppLocalizations.of(context)!.curriculum),
                Tab(text: AppLocalizations.of(context)!.comments),
              ],
            ),
            const Expanded(
              child: TabBarView(
                children: [
                  Padding(
                    padding: EdgeInsets.all(10),
                    child: CurriculumTab(),
                  ),
                  Padding(
                    padding: EdgeInsets.all(10),
                    child: CommentsTab(),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
