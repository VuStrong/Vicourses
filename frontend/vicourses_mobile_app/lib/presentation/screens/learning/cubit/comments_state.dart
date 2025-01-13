part of 'comments_cubit.dart';

enum CreateCommentStatus { idle, pending, success, failed }

class CommentsState {
  final bool isLoading;
  final bool isLoadingMore;
  final List<Comment> comments;
  final bool end;
  final String sort;
  final String? lessonId;
  final CreateCommentStatus createStatus;
  final Comment? replyTo;

  CommentsState({
    this.isLoading = false,
    this.isLoadingMore = false,
    required this.comments,
    this.end = true,
    this.sort = CommentSort.newest,
    this.lessonId,
    this.createStatus = CreateCommentStatus.idle,
    this.replyTo,
  });

  static CommentsState init() => CommentsState(comments: []);

  CommentsState copyWith({
    bool? isLoading,
    bool? isLoadingMore,
    List<Comment>? comments,
    bool? end,
    String? sort,
    String? Function()? lessonId,
    CreateCommentStatus? createStatus,
    Comment? Function()? replyTo,
  }) {
    return CommentsState(
      isLoading: isLoading ?? this.isLoading,
      isLoadingMore: isLoadingMore ?? this.isLoadingMore,
      comments: comments ?? this.comments,
      end: end ?? this.end,
      sort: sort ?? this.sort,
      lessonId: lessonId != null ? lessonId() : this.lessonId,
      createStatus: createStatus ?? this.createStatus,
      replyTo: replyTo != null ? replyTo() : this.replyTo,
    );
  }
}
