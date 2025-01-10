import 'package:vicourses_mobile_app/models/course.dart';

class Wishlist {
  final String id;
  final int count;
  final List<CourseInWishlist> courses;

  Wishlist({
    required this.id,
    this.count = 0,
    required this.courses,
  });

  static Wishlist fromMap(Map<String, dynamic> data) {
    return Wishlist(
      id: data['id'],
      count: data['count'],
      courses: (data['courses'] as List)
          .map((e) => CourseInWishlist.fromMap(e))
          .toList(),
    );
  }
}

class CourseInWishlist {
  final String id;
  final String title;
  final String titleCleaned;
  final bool isPaid;
  final num price;
  final num rating;
  final String? thumbnailUrl;
  final UserInCourse user;

  CourseInWishlist({
    required this.id,
    required this.title,
    required this.titleCleaned,
    required this.isPaid,
    required this.price,
    required this.rating,
    required this.user,
    this.thumbnailUrl,
  });

  static CourseInWishlist fromMap(Map<String, dynamic> data) {
    return CourseInWishlist(
      id: data['id'],
      title: data['title'],
      titleCleaned: data['titleCleaned'],
      isPaid: data['isPaid'],
      price: data['price'],
      rating: data['rating'],
      thumbnailUrl: data['thumbnailUrl'],
      user: UserInCourse(
        id: data['user']['id'],
        name: data['user']['name'],
        thumbnailUrl: data['user']['thumbnailUrl'],
      ),
    );
  }
}
