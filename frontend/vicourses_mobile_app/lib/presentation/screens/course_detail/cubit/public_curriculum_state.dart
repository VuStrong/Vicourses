part of 'public_curriculum_cubit.dart';

class PublicCurriculumState {
  static const int _limit = 5;

  final List<SectionInPublicCurriculum> _sections;

  final bool isLoading;
  final Set<String> expandedSectionIds;
  final bool hide;
  final int totalDuration;
  final int totalSection;
  final int totalLesson;

  List<SectionInPublicCurriculum> get sections =>
      hide ? _sections.take(_limit).toList() : _sections;

  bool get canShowAll => _sections.length > _limit;

  PublicCurriculumState({
    this.isLoading = false,
    required this.expandedSectionIds,
    this.hide = true,
    this.totalDuration = 0,
    this.totalSection = 0,
    this.totalLesson = 0,
    required List<SectionInPublicCurriculum> sections,
  }) : _sections = sections;

  static PublicCurriculumState init() {
    return PublicCurriculumState(
      expandedSectionIds: {},
      sections: [],
    );
  }

  PublicCurriculumState copyWith({
    bool? isLoading,
    Set<String>? expandedSectionIds,
    bool? hide,
    int? totalDuration,
    int? totalSection,
    int? totalLesson,
    List<SectionInPublicCurriculum>? sections,
  }) {
    return PublicCurriculumState(
      isLoading: isLoading ?? this.isLoading,
      expandedSectionIds: expandedSectionIds ?? this.expandedSectionIds,
      hide: hide ?? this.hide,
      totalDuration: totalDuration ?? this.totalDuration,
      totalSection: totalSection ?? this.totalSection,
      totalLesson: totalLesson ?? this.totalLesson,
      sections: sections ?? _sections,
    );
  }
}
