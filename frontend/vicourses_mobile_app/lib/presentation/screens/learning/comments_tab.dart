import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/user/user.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/cubit/comments.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/widgets/comment_bar.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/widgets/comment_item.dart';
import 'package:vicourses_mobile_app/presentation/screens/learning/widgets/sort_comment_button.dart';

class CommentsTab extends StatefulWidget {
  const CommentsTab({super.key});

  @override
  State<CommentsTab> createState() => _CommentsTabState();
}

class _CommentsTabState extends State<CommentsTab>
    with AutomaticKeepAliveClientMixin<CommentsTab> {
  @override
  bool get wantKeepAlive => true;

  @override
  Widget build(BuildContext context) {
    super.build(context);

    return Column(
      children: [
        _sortButtons(),
        const Expanded(
          child: _CommentsList(),
        ),
        const Divider(),
        const CommentBar(),
      ],
    );
  }

  Widget _sortButtons() {
    return BlocBuilder<CommentsCubit, CommentsState>(
      buildWhen: (prev, current) => prev.sort != current.sort,
      builder: (context, state) {
        return SizedBox(
          height: 40,
          child: ListView(
            shrinkWrap: true,
            scrollDirection: Axis.horizontal,
            children: [
              SortCommentButton(
                text: AppLocalizations.of(context)!.newest,
                active: state.sort == CommentSort.newest,
                onPressed: () {
                  context.read<CommentsCubit>().setSort(CommentSort.newest);
                },
              ),
              const SizedBox(width: 4),
              SortCommentButton(
                text: AppLocalizations.of(context)!.oldest,
                active: state.sort == CommentSort.oldest,
                onPressed: () {
                  context.read<CommentsCubit>().setSort(CommentSort.oldest);
                },
              ),
              const SizedBox(width: 4),
              SortCommentButton(
                text: AppLocalizations.of(context)!.highestUpvoted,
                active: state.sort == CommentSort.highestUpvoted,
                onPressed: () {
                  context
                      .read<CommentsCubit>()
                      .setSort(CommentSort.highestUpvoted);
                },
              ),
            ],
          ),
        );
      },
    );
  }
}

class _CommentsList extends StatelessWidget {
  const _CommentsList();

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<CommentsCubit, CommentsState>(
      buildWhen: (prev, current) =>
          prev.isLoading != current.isLoading ||
          prev.comments != current.comments,
      builder: (context, state) {
        if (state.isLoading) {
          return const Center(child: CircularProgressIndicator());
        }
        if (state.comments.isEmpty) {
          return Center(
            child: Text(AppLocalizations.of(context)!.noResults),
          );
        }

        final currentUser = context.read<UserBloc>().state.user;
        final length = state.comments.length;

        return ListView.builder(
          padding: const EdgeInsets.only(top: 10),
          itemCount: length + 1,
          itemBuilder: (context, index) {
            if (index == length) {
              return _loadMoreButton();
            }

            return Container(
              decoration: BoxDecoration(
                border: Border(
                  bottom: BorderSide(
                    width: 1.5,
                    color: Colors.grey.withOpacity(0.2),
                  ),
                ),
              ),
              child: CommentItem(
                comment: state.comments[index],
                currentUser: currentUser,
              ),
            );
          },
        );
      },
    );
  }

  Widget _loadMoreButton() {
    return BlocBuilder<CommentsCubit, CommentsState>(
      buildWhen: (prev, current) =>
          prev.isLoadingMore != current.isLoadingMore ||
          prev.end != current.end,
      builder: (context, state) {
        if (state.end) return const SizedBox.shrink();

        return OutlinedButton(
          onPressed: !state.isLoadingMore
              ? () {
                  context.read<CommentsCubit>().loadMore();
                }
              : null,
          child: state.isLoadingMore
              ? const CircularProgressIndicator()
              : const Icon(Icons.arrow_downward_rounded),
        );
      },
    );
  }
}
