import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/course_ratings.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/rating_item.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

class CourseRatingsSection extends StatelessWidget {
  final CourseDetail course;

  const CourseRatingsSection({
    super.key,
    required this.course,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.studentReviews,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 20,
          ),
        ),
        _rating(),
        _ratingList(),
      ],
    );
  }

  Widget _rating() {
    return BlocBuilder<CourseRatingsCubit, CourseRatingsState>(
      builder: (context, state) {
        return Row(
          children: [
            Text('${course.rating}',
                style: const TextStyle(
                  fontSize: 22,
                  fontWeight: FontWeight.bold,
                )),
            const SizedBox(width: 10),
            Text(
                '${AppLocalizations.of(context)!.rating} (${state.totalRating})'),
          ],
        );
      },
    );
  }

  Widget _ratingList() {
    return BlocBuilder<CourseRatingsCubit, CourseRatingsState>(
      builder: (context, state) {
        if (state.isLoading) {
          return const Center(child: CircularProgressIndicator());
        }

        if (state.ratings.isEmpty) {
          return Center(
            child: Text(AppLocalizations.of(context)!.noResults),
          );
        }

        return Column(
          children: [
            ...state.ratings.map((rating) {
              return Padding(
                padding: const EdgeInsets.symmetric(vertical: 10),
                child: RatingItem(rating: rating),
              );
            }).toList(),
            const SizedBox(height: 10),
            Row(
              children: [
                Expanded(child: _showAllButton(context)),
              ],
            ),
          ],
        );
      },
    );
  }

  Widget _showAllButton(BuildContext context) {
    return OutlinedButton(
      style: OutlinedButton.styleFrom(
        shape: const RoundedRectangleBorder(),
      ),
      onPressed: () {
        context.push(AppRoutes.getCourseRatingsRoute(course.id));
      },
      child: Text(AppLocalizations.of(context)!.showAll),
    );
  }
}
