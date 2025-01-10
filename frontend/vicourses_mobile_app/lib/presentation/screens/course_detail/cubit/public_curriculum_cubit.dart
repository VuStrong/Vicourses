import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/public_curriculum.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'public_curriculum_state.dart';

class PublicCurriculumCubit extends Cubit<PublicCurriculumState> {
  final CourseService _courseService;

  PublicCurriculumCubit(this._courseService)
      : super(PublicCurriculumState.init());

  Future<void> fetchPublicCurriculum(String courseId) async {
    emit(state.copyWith(isLoading: true));

    final curriculum = await _courseService.getPublicCurriculum(courseId);

    emit(state.copyWith(
      isLoading: false,
      totalDuration: curriculum?.totalDuration ?? 0,
      totalSection: curriculum?.totalSection ?? 0,
      totalLesson: curriculum?.totalLesson ?? 0,
      sections: curriculum?.sections ?? [],
    ));
  }

  void toggleExpandSection(int index) {
    final section = state.sections[index];
    final expanded = state.expandedSectionIds.add(section.id);

    if (!expanded) state.expandedSectionIds.remove(section.id);

    emit(state.copyWith());
  }

  void toggleHide() {
    emit(state.copyWith(hide: !state.hide));
  }
}
