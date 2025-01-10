class PublicCurriculum {
  final int totalDuration;
  final int totalSection;
  final int totalLesson;
  final List<SectionInPublicCurriculum> sections;

  PublicCurriculum({
    this.totalDuration = 0,
    this.totalSection = 0,
    this.totalLesson = 0,
    required this.sections,
  });

  static PublicCurriculum fromMap(Map<String, dynamic> data) {
    return PublicCurriculum(
      totalDuration: data['totalDuration'],
      totalSection: data['totalSection'],
      totalLesson: data['totalLesson'],
      sections: (data['sections'] as List)
          .map((e) => SectionInPublicCurriculum.fromMap(e))
          .toList(),
    );
  }
}

class SectionInPublicCurriculum {
  final String id;
  final String title;
  final int order;
  final int duration;
  final int lessonCount;
  final List<LessonInPublicCurriculum> lessons;

  SectionInPublicCurriculum({
    required this.id,
    required this.title,
    required this.order,
    this.duration = 0,
    this.lessonCount = 0,
    required this.lessons,
  });

  static SectionInPublicCurriculum fromMap(Map<String, dynamic> data) {
    return SectionInPublicCurriculum(
      id: data['id'],
      title: data['title'],
      order: data['order'],
      duration: data['duration'],
      lessonCount: data['lessonCount'],
      lessons: (data['lessons'] as List)
          .map((e) => LessonInPublicCurriculum(
                id: e['id'],
                title: e['title'],
                order: e['order'],
                type: e['type'],
                duration: e['duration'],
                quizzesCount: e['quizzesCount'],
              ))
          .toList(),
    );
  }
}

class LessonInPublicCurriculum {
  final String id;
  final String title;
  final int order;
  final String type;
  final int duration;
  final int quizzesCount;

  LessonInPublicCurriculum({
    required this.id,
    required this.title,
    required this.order,
    required this.type,
    this.duration = 0,
    this.quizzesCount = 0,
  });
}
