class Payment {
  final String id;
  final String userId;
  final String username;
  final String email;
  final String courseId;
  final String courseName;
  final String? paypalOrderId;

  Payment({
    required this.id,
    required this.userId,
    required this.username,
    required this.email,
    required this.courseId,
    required this.courseName,
    required this.paypalOrderId,
  });

  static Payment fromMap(Map<String, dynamic> data) {
    return Payment(
      id: data['id'],
      userId: data['userId'],
      username: data['username'],
      email: data['email'],
      courseId: data['courseId'],
      courseName: data['courseName'],
      paypalOrderId: data['paypalOrderId'],
    );
  }
}
