import 'package:vicourses_mobile_app/models/locale.dart';

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
