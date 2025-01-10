import 'package:vicourses_mobile_app/models/quiz.dart';
import 'package:vicourses_mobile_app/models/video.dart';

class Lesson {
  final String id;
  final String courseId;
  final String sectionId;
  final String userId;
  final String title;
  final int order;
  final String type;
  final String? description;
  final Video? video;
  final int quizzesCount;
  final List<Quiz> quizzes;

  Lesson({
    required this.id,
    required this.courseId,
    required this.sectionId,
    required this.userId,
    required this.title,
    required this.order,
    required this.type,
    this.description,
    this.video,
    this.quizzesCount = 0,
    required this.quizzes,
  });

  static Lesson fromMap(Map<String, dynamic> data) {
    return Lesson(
      id: data['id'],
      courseId: data['courseId'],
      sectionId: data['sectionId'],
      userId: data['userId'],
      title: data['title'],
      order: data['order'],
      type: data['type'],
      description: data['description'],
      video: data['video'] != null
          ? Video(
              originalFileName: data['video']['originalFileName'],
              duration: data['video']['duration'],
              status: data['video']['status'],
              token: data['video']['token'],
            )
          : null,
      quizzesCount: data['quizzesCount'],
      quizzes: (data['quizzes'] as List).map((e) => Quiz.fromMap(e)).toList(),
    );
  }
}
