import 'package:vicourses_mobile_app/models/locale.dart';
import 'package:vicourses_mobile_app/models/video.dart';

class Course {
  final String id;
  final String title;
  final String titleCleaned;
  final List<String> learnedContents;
  final String level;
  final bool isPaid;
  final num price;
  final num rating;
  final DateTime createdAt;
  final DateTime updatedAt;
  final int studentCount;
  final Locale locale;
  final String? thumbnailUrl;
  final UserInCourse user;
  final CategoryInCourse category;
  final CategoryInCourse subCategory;
  final List<String> tags;

  Course({
    required this.id,
    required this.title,
    required this.titleCleaned,
    required this.learnedContents,
    required this.level,
    required this.isPaid,
    required this.price,
    required this.rating,
    required this.createdAt,
    required this.updatedAt,
    this.studentCount = 0,
    required this.locale,
    this.thumbnailUrl,
    required this.user,
    required this.category,
    required this.subCategory,
    required this.tags,
  });

  static Course fromMap(Map<String, dynamic> data) {
    return Course(
      id: data['id'],
      title: data['title'],
      titleCleaned: data['titleCleaned'],
      learnedContents: List<String>.from(data['learnedContents']),
      level: data['level'],
      isPaid: data['isPaid'],
      price: data['price'],
      rating: data['rating'],
      createdAt: DateTime.parse(data['createdAt']),
      updatedAt: DateTime.parse(data['updatedAt']),
      studentCount: data['studentCount'],
      locale: Locale(
        name: data['locale']['name'],
        englishTitle: data['locale']['englishTitle'],
      ),
      thumbnailUrl: data['thumbnailUrl'],
      user: UserInCourse(
        id: data['user']['id'],
        name: data['user']['name'],
        thumbnailUrl: data['user']['thumbnailUrl'],
      ),
      category: CategoryInCourse(
        id: data['category']['id'],
        name: data['category']['name'],
        slug: data['category']['slug'],
      ),
      subCategory: CategoryInCourse(
        id: data['subCategory']['id'],
        name: data['subCategory']['name'],
        slug: data['subCategory']['slug'],
      ),
      tags: List<String>.from(data['tags']),
    );
  }
}

class CourseDetail extends Course {
  final String? description;
  final List<String> requirements;
  final List<String> targetStudents;
  final Video? previewVideo;
  final CourseMetrics metrics;

  CourseDetail({
    required super.id,
    required super.title,
    required super.titleCleaned,
    required super.learnedContents,
    required super.level,
    required super.isPaid,
    required super.price,
    required super.rating,
    required super.createdAt,
    required super.updatedAt,
    super.studentCount = 0,
    required super.locale,
    super.thumbnailUrl,
    required super.user,
    required super.category,
    required super.subCategory,
    required super.tags,
    this.description,
    required this.requirements,
    required this.targetStudents,
    this.previewVideo,
    required this.metrics,
  });

  static CourseDetail fromMap(Map<String, dynamic> data) {
    return CourseDetail(
      id: data['id'],
      title: data['title'],
      titleCleaned: data['titleCleaned'],
      learnedContents: List<String>.from(data['learnedContents']),
      level: data['level'],
      isPaid: data['isPaid'],
      price: data['price'],
      rating: data['rating'],
      createdAt: DateTime.parse(data['createdAt']),
      updatedAt: DateTime.parse(data['updatedAt']),
      studentCount: data['studentCount'],
      locale: Locale(
        name: data['locale']['name'],
        englishTitle: data['locale']['englishTitle'],
      ),
      thumbnailUrl: data['thumbnailUrl'],
      user: UserInCourse(
        id: data['user']['id'],
        name: data['user']['name'],
        thumbnailUrl: data['user']['thumbnailUrl'],
      ),
      category: CategoryInCourse(
        id: data['category']['id'],
        name: data['category']['name'],
        slug: data['category']['slug'],
      ),
      subCategory: CategoryInCourse(
        id: data['subCategory']['id'],
        name: data['subCategory']['name'],
        slug: data['subCategory']['slug'],
      ),
      tags: List<String>.from(data['tags']),
      description: data['description'],
      requirements: List<String>.from(data['requirements']),
      targetStudents: List<String>.from(data['targetStudents']),
      previewVideo: data['previewVideo'] != null
          ? Video(
              originalFileName: data['previewVideo']['originalFileName'],
              duration: data['previewVideo']['duration'],
              status: data['previewVideo']['status'],
              token: data['previewVideo']['token'],
            )
          : null,
      metrics: CourseMetrics(
        sectionsCount: data['metrics']['sectionsCount'],
        lessonsCount: data['metrics']['lessonsCount'],
        quizLessonsCount: data['metrics']['quizLessonsCount'],
        totalVideoDuration: data['metrics']['totalVideoDuration'],
      ),
    );
  }
}

class UserInCourse {
  final String id;
  final String name;
  final String? thumbnailUrl;

  UserInCourse({
    required this.id,
    required this.name,
    this.thumbnailUrl,
  });
}

class CategoryInCourse {
  final String id;
  final String name;
  final String slug;

  CategoryInCourse({
    required this.id,
    required this.name,
    required this.slug,
  });
}

class CourseMetrics {
  final int sectionsCount;
  final int lessonsCount;
  final int quizLessonsCount;
  final int totalVideoDuration;

  CourseMetrics({
    this.sectionsCount = 0,
    this.lessonsCount = 0,
    this.quizLessonsCount = 0,
    this.totalVideoDuration = 0,
  });
}
