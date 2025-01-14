class Comment {
  final String id;
  final String lessonId;
  final String instructorId;
  final UserInComment user;
  final String content;
  final DateTime createdAt;
  bool isDeleted;
  final String? replyToId;
  int replyCount;
  int upvoteCount;
  List<String> userUpvoteIds;
  List<Comment> replies;

  Comment({
    required this.id,
    required this.lessonId,
    required this.instructorId,
    required this.user,
    required this.content,
    required this.createdAt,
    this.isDeleted = false,
    this.replyToId,
    this.replyCount = 0,
    this.upvoteCount = 0,
    required this.userUpvoteIds,
    required this.replies,
  });

  static Comment fromMap(Map<String, dynamic> data) {
    return Comment(
      id: data['id'],
      lessonId: data['lessonId'],
      instructorId: data['instructorId'],
      user: UserInComment(
        id: data['user']['id'],
        name: data['user']['name'],
        thumbnailUrl: data['user']['thumbnailUrl'],
      ),
      content: data['content'],
      createdAt: DateTime.parse(data['createdAt']),
      isDeleted: data['isDeleted'],
      replyToId: data['replyToId'],
      replyCount: data['replyCount'],
      upvoteCount: data['upvoteCount'],
      userUpvoteIds: List<String>.from(data['userUpvoteIds']),
      replies: [],
    );
  }
}

class UserInComment {
  final String id;
  final String name;
  final String? thumbnailUrl;

  UserInComment({
    required this.id,
    required this.name,
    this.thumbnailUrl,
  });
}
