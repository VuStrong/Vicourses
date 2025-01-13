import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/lesson.dart';
import 'package:vicourses_mobile_app/models/public_curriculum.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'learning_state.dart';

class LearningCubit extends Cubit<LearningState> {
  final CourseService _courseService;

  LearningCubit(this._courseService) : super(LearningState.init());

  Future<void> fetchCurriculum(String courseId) async {
    emit(state.copyWith(isLoadingCurriculum: true));

    final curriculum = await _courseService.getPublicCurriculum(courseId);

    emit(state.copyWith(
      isLoadingCurriculum: false,
      curriculum: () => curriculum,
      lessonIds:
          curriculum != null ? _getLessonIdsFromCurriculum(curriculum) : [],
    ));

    if (curriculum != null &&
        curriculum.sections.isNotEmpty &&
        curriculum.sections[0].lessons.isNotEmpty) {
      jumpToIndex(0);
    }
  }

  Future<void> jumpToIndex(int index) async {
    final lessonId = state._lessonIds[index];

    emit(state.copyWith(
      isLoadingCurrentLesson: true,
      currentIndex: index,
    ));

    final lesson = await _courseService.getLesson(lessonId);

    emit(state.copyWith(
      isLoadingCurrentLesson: false,
      currentLesson: () => lesson,
    ));
  }

  Future<void> jumpToLesson(String lessonId) async {
    final index = state._lessonIds.indexOf(lessonId);

    if (index < 0 || index == state.currentIndex) return;

    await jumpToIndex(index);
  }

  Future<void> moveToPrevLesson() async {
    if (!state.canMoveToPrev) return;

    await jumpToIndex(state.currentIndex - 1);
  }

  Future<void> moveToNextLesson() async {
    if (!state.canMoveToNext) return;

    await jumpToIndex(state.currentIndex + 1);
  }

  List<String> _getLessonIdsFromCurriculum(PublicCurriculum curriculum) {
    List<String> ids = [];

    for (var section in curriculum.sections) {
      for (var lesson in section.lessons) {
        ids.add(lesson.id);
      }
    }

    return ids;
  }
}
