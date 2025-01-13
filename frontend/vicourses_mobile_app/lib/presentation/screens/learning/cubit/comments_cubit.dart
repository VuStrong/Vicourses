import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/comment.dart';
import 'package:vicourses_mobile_app/services/api/comment_service.dart';
import 'package:vicourses_mobile_app/utils/app_exception.dart';

part 'comments_state.dart';

class CommentSort {
  static const String newest = 'Newest';
  static const String oldest = 'Oldest';
  static const String highestUpvoted = 'HighestUpvoted';
}

class CommentsCubit extends Cubit<CommentsState> {
  final CommentService _commentService;

  CommentsCubit(this._commentService) : super(CommentsState.init());

  void setLessonId(String lessonId, {bool fetch = true}) {
    if (lessonId == state.lessonId) return;

    emit(state.copyWith(
      lessonId: () => lessonId,
    ));

    if (fetch) fetchComments();
  }

  void setSort(String sort, {bool fetch = true}) {
    if (sort == state.sort) return;

    emit(state.copyWith(sort: sort));

    if (fetch) fetchComments();
  }

  Future<void> fetchComments() async {
    if (state.lessonId == null || state.isLoading) return;

    emit(state.copyWith(
      isLoading: true,
    ));

    final result = await _commentService.getComments(
      state.lessonId!,
      sort: state.sort,
    );

    emit(state.copyWith(
      isLoading: false,
      comments: result?.items ?? [],
      end: result?.end ?? true,
    ));
  }

  Future<void> loadMore() async {
    if (state.lessonId == null || state.isLoadingMore || state.isLoading) {
      return;
    }

    emit(state.copyWith(isLoadingMore: true));

    final skip = state.comments.length;
    final result = await _commentService.getComments(
      state.lessonId!,
      skip: skip,
      sort: state.sort,
    );

    if (result != null) {
      emit(state.copyWith(
        isLoadingMore: false,
        end: result.end,
        comments: [...state.comments, ...result.items],
      ));
      return;
    }

    emit(state.copyWith(isLoadingMore: false));
  }

  Future<void> createComment({
    required String content,
  }) async {
    if (state.lessonId == null ||
        state.createStatus == CreateCommentStatus.pending) return;

    emit(state.copyWith(
      createStatus: CreateCommentStatus.pending,
    ));

    try {
      final createdComment = await _commentService.createComment(
        lessonId: state.lessonId!,
        content: content,
        replyToId: state.replyTo?.id,
      );

      if (createdComment.replyToId == null) {
        emit(state.copyWith(
          createStatus: CreateCommentStatus.success,
          comments: [createdComment, ...state.comments],
        ));
      } else {
        final newList = state.comments.map((c) {
          if (c.id == createdComment.replyToId) {
            c.replies.add(createdComment);
            c.replyCount++;
          }
          return c;
        }).toList();

        emit(state.copyWith(
          createStatus: CreateCommentStatus.success,
          comments: newList,
        ));
      }
    } on AppException {
      emit(state.copyWith(
        createStatus: CreateCommentStatus.failed,
      ));
    }
  }

  Future<void> deleteComment(Comment comment) async {
    comment.isDeleted = true;

    emit(state.copyWith(comments: [...state.comments]));

    await _commentService.deleteComment(state.lessonId ?? '', comment.id);
  }

  Future<void> upvoteComment(Comment comment, String userId) async {
    if (comment.userUpvoteIds.contains(userId)) return;

    comment.upvoteCount++;
    comment.userUpvoteIds.add(userId);

    emit(state.copyWith(comments: [...state.comments]));

    await _commentService.upvoteComment(state.lessonId ?? '', comment.id);
  }

  Future<void> cancelUpvoteComment(Comment comment, String userId) async {
    if (!comment.userUpvoteIds.contains(userId)) return;

    comment.upvoteCount--;
    comment.userUpvoteIds.remove(userId);

    emit(state.copyWith(comments: [...state.comments]));

    await _commentService.cancelUpvoteComment(state.lessonId ?? '', comment.id);
  }

  Future<void> fetchReplies(String commentId) async {
    if (state.lessonId == null || state.isLoading) return;

    final index = state.comments.indexWhere((c) => c.id == commentId);
    if (index < 0) return;

    final comment = state.comments[index];

    final result = await _commentService.getComments(
      state.lessonId!,
      skip: comment.replies.length,
      limit: 3,
      replyToId: comment.id,
    );

    if (result != null) {
      comment.replies.addAll(result.items);

      emit(state.copyWith(
        comments: [...state.comments],
      ));
    }
  }

  void setReply(Comment? replyTo) {
    if (replyTo == state.replyTo) return;

    emit(state.copyWith(
      replyTo: () => replyTo,
    ));
  }
}
