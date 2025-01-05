import 'package:vicourses_mobile_app/utils/role.dart';

class User {
  final String id;
  final DateTime createdAt;
  final String name;
  final String email;
  final bool emailConfirmed;
  final String role;
  final String? thumbnailUrl;
  final String? headline;
  final String? description;
  final String? websiteUrl;
  final String? youtubeUrl;
  final String? facebookUrl;
  final String? linkedInUrl;
  final bool enrolledCoursesVisible;
  final bool isPublic;
  final int totalEnrollmentCount;

  User({
    required this.id,
    required this.createdAt,
    required this.name,
    required this.email,
    this.emailConfirmed = false,
    required this.role,
    this.thumbnailUrl,
    this.headline,
    this.description,
    this.websiteUrl,
    this.youtubeUrl,
    this.facebookUrl,
    this.linkedInUrl,
    this.enrolledCoursesVisible = true,
    this.isPublic = true,
    this.totalEnrollmentCount = 0,
  });

  static User fromMap(Map<String, dynamic> data) {
    return User(
      id: data['id'] ?? '',
      createdAt: data['createdAt'] != null
          ? DateTime.parse(data['createdAt'])
          : DateTime.now(),
      name: data['name'] ?? '',
      email: data['email'] ?? '',
      emailConfirmed: data['emailConfirmed'] ?? false,
      role: data['role'] ?? Role.student,
      thumbnailUrl: data['thumbnailUrl'],
      headline: data['headline'],
      description: data['description'],
      websiteUrl: data['websiteUrl'],
      youtubeUrl: data['youtubeUrl'],
      facebookUrl: data['facebookUrl'],
      linkedInUrl: data['linkedInUrl'],
      enrolledCoursesVisible: data['enrolledCoursesVisible'] ?? true,
      isPublic: data['isPublic'] ?? true,
      totalEnrollmentCount: data['totalEnrollmentCount'] ?? 0,
    );
  }
}
