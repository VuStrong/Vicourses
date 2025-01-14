part of 'learning_cubit.dart';

class LearningState {
  late final List<String> _lessonIds;

  final bool isLoadingCurriculum;
  final bool isLoadingCurrentLesson;
  final PublicCurriculum? curriculum;
  final Lesson? currentLesson;
  final int currentIndex;

  String? get currentLessonId => _lessonIds.isNotEmpty ? _lessonIds[currentIndex] : null;

  bool get canMoveToPrev => currentIndex > 0;
  bool get canMoveToNext => currentIndex < _lessonIds.length - 1;

  LearningState({
    required List<String> lessonIds,
    this.isLoadingCurriculum = false,
    this.isLoadingCurrentLesson = false,
    this.curriculum,
    this.currentLesson,
    this.currentIndex = 0,
  }) : _lessonIds = lessonIds;

  static LearningState init() => LearningState(lessonIds: []);

  LearningState copyWith({
    bool? isLoadingCurriculum,
    bool? isLoadingCurrentLesson,
    PublicCurriculum? Function()? curriculum,
    Lesson? Function()? currentLesson,
    List<String>? lessonIds,
    int? currentIndex,
  }) {
    return LearningState(
      isLoadingCurriculum: isLoadingCurriculum ?? this.isLoadingCurriculum,
      isLoadingCurrentLesson: isLoadingCurrentLesson ?? this.isLoadingCurrentLesson,
      curriculum: curriculum != null ? curriculum() : this.curriculum,
      currentLesson: currentLesson != null ? currentLesson() : this.currentLesson,
      lessonIds: lessonIds ?? _lessonIds,
      currentIndex: currentIndex ?? this.currentIndex,
    );
  }
}
