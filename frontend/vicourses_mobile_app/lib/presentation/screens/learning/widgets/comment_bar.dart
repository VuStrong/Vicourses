import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/snack_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/comments.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class CommentBar extends StatefulWidget {
  const CommentBar({super.key});

  @override
  State<CommentBar> createState() => _CommentBarState();
}

class _CommentBarState extends State<CommentBar> {
  late final TextEditingController _commentController;
  late final FocusNode _commentFocusNode;

  @override
  void initState() {
    _commentController = TextEditingController();
    _commentFocusNode = FocusNode();
    super.initState();
  }

  @override
  void dispose() {
    _commentController.dispose();
    _commentFocusNode.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return MultiBlocListener(
      listeners: [
        BlocListener<CommentsCubit, CommentsState>(
          listenWhen: (prev, current) =>
              prev.createStatus != current.createStatus,
          listener: (context, state) {
            if (state.createStatus == CreateCommentStatus.success) {
              _commentController.text = '';
              return;
            }

            if (state.createStatus == CreateCommentStatus.failed) {
              return showSnackBar(
                context: context,
                text: AppLocalizations.of(context)!.errorOccurred,
                type: SnackBarType.error,
              );
            }
          },
        ),
        BlocListener<CommentsCubit, CommentsState>(
          listenWhen: (prev, current) =>
              prev.replyTo != current.replyTo,
          listener: (context, state) {
            if (state.replyTo != null) {
              _commentFocusNode.requestFocus();
            }
          },
        ),
      ],
      child: Column(
        children: [
          _reply(),
          Row(
            children: [
              _userThumbnail(),
              const SizedBox(width: 4),
              Expanded(child: _commentField(context)),
              _submitButton(),
            ],
          ),
        ],
      ),
    );
  }

  Widget _reply() {
    return BlocBuilder<CommentsCubit, CommentsState>(
      buildWhen: (prev, current) => prev.replyTo != current.replyTo,
      builder: (context, state) {
        if (state.replyTo == null) {
          return const SizedBox.shrink();
        }

        final localizations = AppLocalizations.of(context)!;
        return Row(
          children: [
            Text(
              localizations.replyTo(state.replyTo!.user.name),
            ),
            TextButton(
              onPressed: () {
                context.read<CommentsCubit>().setReply(null);
              },
              child: Text(localizations.cancel),
            ),
          ],
        );
      },
    );
  }

  Widget _userThumbnail() {
    return BlocBuilder<UserBloc, UserState>(
      builder: (context, state) {
        final thumbnailUrl = state.user?.thumbnailUrl;

        return SizedBox(
          width: 30,
          height: 30,
          child: ClipOval(
            child: thumbnailUrl != null
                ? Image.network(
                    thumbnailUrl,
                    fit: BoxFit.cover,
                  )
                : Image.asset(
                    AppConstants.defaultUserImagePath,
                    fit: BoxFit.cover,
                  ),
          ),
        );
      },
    );
  }

  Widget _commentField(BuildContext context) {
    return TextField(
      controller: _commentController,
      focusNode: _commentFocusNode,
      decoration: InputDecoration(
        hintText: AppLocalizations.of(context)!.writeComment,
      ),
      onTapOutside: (_) => FocusScope.of(context).unfocus(),
    );
  }

  Widget _submitButton() {
    return BlocBuilder<CommentsCubit, CommentsState>(
      buildWhen: (prev, current) => prev.createStatus != current.createStatus,
      builder: (context, state) {
        return IconButton(
          onPressed: state.createStatus != CreateCommentStatus.pending
              ? () => _onSubmitComment(context)
              : null,
          icon: state.createStatus == CreateCommentStatus.pending
              ? const SizedBox(
                  width: 30,
                  height: 30,
                  child: CircularProgressIndicator(),
                )
              : const Icon(Icons.send),
        );
      },
    );
  }

  void _onSubmitComment(BuildContext context) {
    final content = _commentController.text.trim();

    if (content.isNotEmpty) {
      context.read<CommentsCubit>().createComment(
            content: content,
          );
    }
  }
}
