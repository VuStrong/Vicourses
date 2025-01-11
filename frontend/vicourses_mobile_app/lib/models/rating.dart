class Rating {
  final String id;
  final String courseId;
  final UserInRating user;
  final String feedback;
  final num star;
  final DateTime createdAt;
  final bool responded;
  final String? response;
  final DateTime? respondedAt;

  Rating({
    required this.id,
    required this.courseId,
    required this.user,
    required this.feedback,
    required this.star,
    required this.createdAt,
    this.responded = false,
    this.response,
    this.respondedAt,
  });

  static Rating fromMap(Map<String, dynamic> data) {
    return Rating(
      id: data['id'],
      courseId: data['courseId'],
      user: UserInRating(
        id: data['user']['id'],
        name: data['user']['name'],
        thumbnailUrl: data['user']['thumbnailUrl'],
      ),
      feedback: data['feedback'],
      star: data['star'],
      createdAt: DateTime.parse(data['createdAt']),
      responded: data['responded'],
      response: data['response'],
      respondedAt: data['respondedAt'] != null
          ? DateTime.parse(data['respondedAt'])
          : null,
    );
  }
}

class UserInRating {
  final String id;
  final String name;
  final String? thumbnailUrl;

  UserInRating({
    required this.id,
    required this.name,
    this.thumbnailUrl,
  });
}
