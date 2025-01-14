import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/public_curriculum.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/learning.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';
import 'package:vicourses_mobile_app/utils/extensions.dart';

class CurriculumTab extends StatefulWidget {
  const CurriculumTab({super.key});

  @override
  State<CurriculumTab> createState() => _CurriculumTabState();
}

class _CurriculumTabState extends State<CurriculumTab>
    with AutomaticKeepAliveClientMixin<CurriculumTab> {
  @override
  bool get wantKeepAlive => true;

  @override
  Widget build(BuildContext context) {
    super.build(context);

    return BlocBuilder<LearningCubit, LearningState>(
      buildWhen: (prev, current) =>
          prev.isLoadingCurriculum != current.isLoadingCurriculum ||
          prev.curriculum != current.curriculum ||
          prev.currentIndex != current.currentIndex,
      builder: (context, state) {
        if (state.isLoadingCurriculum) {
          return const Center(child: CircularProgressIndicator());
        }

        if (state.curriculum == null || state.curriculum!.sections.isEmpty) {
          return Center(
            child: Text(AppLocalizations.of(context)!.noResults),
          );
        }

        return SingleChildScrollView(
          child: _sectionList(context, state),
        );
      },
    );
  }

  Widget _sectionList(BuildContext context, LearningState state) {
    final sectionStr = AppLocalizations.of(context)!.section;
    int index = 0;

    return Column(
      children: [
        ...state.curriculum!.sections.map((section) {
          index++;
          int curIndex = index;

          return Column(
            children: [
              ListTile(
                title: Text(
                  '$sectionStr $curIndex: ${section.title}',
                  style: const TextStyle(
                    fontWeight: FontWeight.w500,
                  ),
                ),
                contentPadding: EdgeInsets.zero,
              ),
              ...section.lessons.map((lesson) {
                return _lessonItem(
                  context,
                  lesson,
                  active: state.currentLessonId == lesson.id,
                );
              }),
            ],
          );
        }),
        const SizedBox(height: 100),
      ],
    );
  }

  Widget _lessonItem(
    BuildContext context,
    LessonInPublicCurriculum lesson, {
    required bool active,
  }) {
    final subtitle = lesson.type == LessonType.video
        ? '${lesson.type} - ${lesson.duration.toDurationString()}'
        : '${lesson.type} - ${lesson.quizzesCount} ${AppLocalizations.of(context)!.quizzes}';

    return ListTile(
      leading: Text(
        '${lesson.order}',
        style: const TextStyle(fontSize: 16),
      ),
      title: Text(
        lesson.title,
        overflow: TextOverflow.ellipsis,
      ),
      subtitle: Text(subtitle),
      visualDensity: const VisualDensity(horizontal: 0, vertical: -4),
      tileColor:
          active ? Theme.of(context).primaryColor.withOpacity(0.2) : null,
      onTap: () {
        context.read<LearningCubit>().jumpToLesson(lesson.id);
      },
    );
  }
}
