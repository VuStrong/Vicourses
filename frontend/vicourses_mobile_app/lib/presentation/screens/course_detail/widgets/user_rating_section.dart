import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/rating.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/star_rating.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/course_detail.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/user_rating.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/rating_item.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

class UserRatingSection extends StatelessWidget {
  final String courseId;

  const UserRatingSection({
    super.key,
    required this.courseId,
  });

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<CourseDetailCubit, CourseDetailState>(
      buildWhen: (prev, current) => current.enrolled,
      builder: (context, state) {
        if (!state.enrolled) return const SizedBox.shrink();

        return _ratingBuilder();
      },
    );
  }

  Widget _ratingBuilder() {
    return BlocBuilder<UserRatingCubit, UserRatingState>(
      buildWhen: (prev, current) =>
          prev.isLoading != current.isLoading || prev.rating != current.rating,
      builder: (context, state) {
        if (state.isLoading) {
          return const Center(child: CircularProgressIndicator());
        }

        if (state.rating == null) {
          return _createRatingWidget(context);
        }

        return _userRating(context, state.rating!);
      },
    );
  }

  Widget _createRatingWidget(BuildContext context) {
    return Column(
      children: [
        const StarRating(
          initialRating: 0,
          itemSize: 30,
          readonly: true,
        ),
        TextButton(
          onPressed: () {
            context.push(
              AppRoutes.getEditRatingRoute(courseId),
              extra: context.read<UserRatingCubit>(),
            );
          },
          child: Text(AppLocalizations.of(context)!.writeFeedback),
        ),
      ],
    );
  }

  Widget _userRating(BuildContext context, Rating rating) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.yourRating,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        RatingItem(
          rating: rating,
          action: PopupMenuButton<int>(
            onSelected: (item) {
              if (item == 0) {
                context.push(
                  AppRoutes.getEditRatingRoute(courseId),
                  extra: context.read<UserRatingCubit>(),
                );
              } else if (item == 1) {
                context.read<UserRatingCubit>().deleteRating();
              }
            },
            itemBuilder: (context) => [
              PopupMenuItem<int>(
                value: 0,
                child: Text(AppLocalizations.of(context)!.edit),
              ),
              PopupMenuItem<int>(
                value: 1,
                child: Text(AppLocalizations.of(context)!.delete),
              ),
            ],
          ),
        ),
        const SizedBox(height: 10),
      ],
    );
  }
}
