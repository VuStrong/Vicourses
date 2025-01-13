import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_html/flutter_html.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/comment.dart';
import 'package:vicourses_mobile_app/models/user.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/comments.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class CommentItem extends StatelessWidget {
  final Comment comment;
  final User? currentUser;

  const CommentItem({
    super.key,
    required this.comment,
    this.currentUser,
  });

  @override
  Widget build(BuildContext context) {
    final createdAt =
        '${comment.createdAt.day}/${comment.createdAt.month}/${comment.createdAt.year}';
    final upvoted = comment.userUpvoteIds.contains(currentUser?.id);

    return Column(
      children: [
        ListTile(
          isThreeLine: true,
          contentPadding: EdgeInsets.zero,
          leading: _thumbnail(),
          title: Text(
            '${comment.user.name} - $createdAt',
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 13,
            ),
            overflow: TextOverflow.ellipsis,
          ),
          subtitle: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              _content(context),
              if (!comment.isDeleted)
                Row(
                  children: [
                    _upvoteButton(context, upvoted),
                    _replyButton(context),
                  ],
                ),
            ],
          ),
          trailing: _menuButton(context),
        ),
        if (comment.replies.isNotEmpty)
          Padding(
            padding: const EdgeInsets.only(left: 40),
            child: Column(
              children: comment.replies.map((reply) {
                return CommentItem(
                  comment: reply,
                  currentUser: currentUser,
                );
              }).toList(),
            ),
          ),
        _loadRepliesButton(context),
      ],
    );
  }

  Widget _thumbnail() {
    return SizedBox(
      width: 30,
      height: 30,
      child: ClipOval(
        child: comment.user.thumbnailUrl != null
            ? Image.network(
                comment.user.thumbnailUrl!,
                fit: BoxFit.cover,
              )
            : Image.asset(
                AppConstants.defaultUserImagePath,
                fit: BoxFit.cover,
              ),
      ),
    );
  }

  Widget _content(BuildContext context) {
    if (comment.isDeleted) {
      return Text(
        AppLocalizations.of(context)!.deleted,
        style: const TextStyle(
          fontWeight: FontWeight.bold,
          fontStyle: FontStyle.italic,
          decoration: TextDecoration.lineThrough,
        ),
      );
    }

    return Html(
      data: comment.content,
      style: {
        'body': Style(
          margin: Margins.all(0),
          padding: HtmlPaddings.zero,
          fontSize: FontSize.large,
        ),
        "p": Style(
          padding: HtmlPaddings.zero,
          margin: Margins.zero,
        ),
      },
    );
  }

  Widget _upvoteButton(BuildContext context, bool upvoted) {
    return Row(
      children: [
        IconButton(
          onPressed: () {
            if (currentUser == null) return;
            if (upvoted) {
              context
                  .read<CommentsCubit>()
                  .cancelUpvoteComment(comment, currentUser!.id);
            } else {
              context
                  .read<CommentsCubit>()
                  .upvoteComment(comment, currentUser!.id);
            }
          },
          icon: Icon(
            Icons.arrow_circle_up,
            color: upvoted ? Theme.of(context).primaryColor : null,
          ),
          padding: EdgeInsets.zero,
        ),
        Text('${comment.upvoteCount}')
      ],
    );
  }

  Widget _replyButton(BuildContext context) {
    if (comment.replyToId != null) return const SizedBox.shrink();

    return Row(
      children: [
        IconButton(
          onPressed: () {
            context.read<CommentsCubit>().setReply(comment);
          },
          icon: const Icon(Icons.reply),
          padding: EdgeInsets.zero,
        ),
        Text('${comment.replyCount}')
      ],
    );
  }

  Widget? _menuButton(BuildContext context) {
    final allowDelete = currentUser?.id == comment.user.id ||
        currentUser?.id == comment.instructorId;

    if (!allowDelete || comment.isDeleted) {
      return null;
    }

    return PopupMenuButton<int>(
      onSelected: (item) {
        if (item == 0) {
          context.read<CommentsCubit>().deleteComment(comment);
        }
      },
      itemBuilder: (context) => [
        PopupMenuItem<int>(
          value: 0,
          child: Text(AppLocalizations.of(context)!.delete),
        ),
      ],
    );
  }

  Widget _loadRepliesButton(BuildContext context) {
    if (comment.replies.length >= comment.replyCount) {
      return const SizedBox.shrink();
    }

    return TextButton(
      onPressed: () {
        context.read<CommentsCubit>().fetchReplies(comment.id);
      },
      child: Text(AppLocalizations.of(context)!.showReplies),
    );
  }
}
