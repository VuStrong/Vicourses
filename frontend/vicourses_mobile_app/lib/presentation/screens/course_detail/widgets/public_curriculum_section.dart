import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/public_curriculum.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/public_curriculum.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';
import 'package:vicourses_mobile_app/utils/extensions.dart';

class PublicCurriculumSection extends StatelessWidget {
  const PublicCurriculumSection({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.curriculum,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        _metrics(),
        _curriculumExpansionList(),
      ],
    );
  }

  Widget _metrics() {
    return BlocBuilder<PublicCurriculumCubit, PublicCurriculumState>(
      buildWhen: (prev, current) =>
          prev.totalSection != current.totalSection ||
          prev.totalLesson != current.totalLesson ||
          prev.totalDuration != current.totalDuration,
      builder: (context, state) {
        final totalSection = state.totalSection;
        final totalLesson = state.totalLesson;
        final totalDuration = state.totalDuration;

        return Row(
          children: [
            Text(
              '$totalSection ${AppLocalizations.of(context)!.sections}',
            ),
            const Text(' • '),
            Text(
              '$totalLesson ${AppLocalizations.of(context)!.lessons}',
            ),
            const Text(' • '),
            Text(
              totalDuration.toDurationString(),
            ),
          ],
        );
      },
    );
  }

  Widget _curriculumExpansionList() {
    return BlocBuilder<PublicCurriculumCubit, PublicCurriculumState>(
      builder: (context, state) {
        if (state.isLoading) {
          return const Center(child: CircularProgressIndicator());
        }

        if (state.sections.isEmpty) {
          return const SizedBox();
        }

        return Column(
          children: [
            SectionExpansionList(
              sections: state.sections,
              expandedSectionIds: state.expandedSectionIds,
              onTap: (index) {
                context
                    .read<PublicCurriculumCubit>()
                    .toggleExpandSection(index);
              },
            ),
            if (state.canShowAll)
              Row(
                children: [
                  Expanded(child: _showAllButton(context, state.hide)),
                ],
              ),
          ],
        );
      },
    );
  }

  Widget _showAllButton(BuildContext context, bool hide) {
    return OutlinedButton(
      style: OutlinedButton.styleFrom(
        shape: const RoundedRectangleBorder(),
      ),
      onPressed: () {
        context.read<PublicCurriculumCubit>().toggleHide();
      },
      child: Text(
        hide
            ? AppLocalizations.of(context)!.showAll
            : AppLocalizations.of(context)!.hide,
      ),
    );
  }
}

class SectionExpansionList extends StatelessWidget {
  final List<SectionInPublicCurriculum> sections;
  final Set<String> expandedSectionIds;
  final void Function(int) onTap;

  const SectionExpansionList({
    super.key,
    required this.sections,
    required this.expandedSectionIds,
    required this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    int index = 0;
    final sectionStr = AppLocalizations.of(context)!.section;

    return ExpansionPanelList(
      elevation: 0,
      expansionCallback: (int index, _) {
        onTap(index);
      },
      children: sections.map((section) {
        index++;
        int curIndex = index;

        return ExpansionPanel(
          canTapOnHeader: true,
          isExpanded: expandedSectionIds.contains(section.id),
          headerBuilder: (BuildContext context, _) {
            return ListTile(
              contentPadding: EdgeInsets.zero,
              title: Text(
                '$sectionStr $curIndex: ${section.title}',
                style: const TextStyle(
                  fontWeight: FontWeight.w500,
                ),
              ),
            );
          },
          body: Column(
            children: section.lessons.map((lesson) {
              return _lessonItem(context, lesson);
            }).toList(),
          ),
        );
      }).toList(),
    );
  }

  Widget _lessonItem(BuildContext context, LessonInPublicCurriculum lesson) {
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
    );
  }
}
