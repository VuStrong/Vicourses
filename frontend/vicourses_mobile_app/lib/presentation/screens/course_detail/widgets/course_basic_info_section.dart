import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/star_rating.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class CourseBasicInfoSection extends StatelessWidget {
  final CourseDetail course;

  const CourseBasicInfoSection({
    super.key,
    required this.course,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        _courseThumbnail(context),
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 10),
          child: Text(
            course.title,
            style: const TextStyle(fontSize: 22),
          ),
        ),
        _rating(context),
        const SizedBox(height: 10),
        _instructorLink(context),
        _lastUpdated(context),
        _locale(context),
        const SizedBox(height: 10),
        _tagsList(context),
      ],
    );
  }

  Widget _courseThumbnail(BuildContext context) {
    final image = course.thumbnailUrl != null
        ? NetworkImage(course.thumbnailUrl!)
        : const AssetImage(AppConstants.defaultCourseImagePath)
            as ImageProvider;

    return AspectRatio(
      aspectRatio: 16 / 9,
      child: Container(
        decoration: BoxDecoration(
          image: DecorationImage(
            image: image,
            fit: BoxFit.cover,
            colorFilter: ColorFilter.mode(
              Colors.black.withOpacity(0.2),
              BlendMode.darken,
            ),
          ),
        ),
        child: Center(
          child: IconButton(
            onPressed: () {
              //
            },
            icon: const Icon(
              Icons.play_arrow,
              size: 100,
              color: Colors.white,
            ),
          ),
        ),
      ),
    );
  }

  Widget _rating(BuildContext context) {
    return Row(
      children: [
        Text(
          course.rating.toString(),
          style: const TextStyle(
            fontWeight: FontWeight.w600,
          ),
        ),
        const SizedBox(width: 4),
        StarRating(
          initialRating: course.rating.toDouble(),
          readonly: true,
          itemSize: 18,
        ),
        const Padding(
          padding: EdgeInsets.symmetric(horizontal: 5),
          child: Icon(Icons.circle, size: 5),
        ),
        Text(
          '${course.studentCount} ${AppLocalizations.of(context)!.students}',
        ),
      ],
    );
  }

  Widget _instructorLink(BuildContext context) {
    return Row(
      children: [
        Text(AppLocalizations.of(context)!.createdBy),
        InkWell(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 5),
            child: Text(
              course.user.name,
              style: TextStyle(
                color: Theme.of(context).primaryColor,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          onTap: () =>
              context.push(AppRoutes.getUserProfileRoute(course.user.id)),
        ),
      ],
    );
  }

  Widget _lastUpdated(BuildContext context) {
    final updatedAt = course.updatedAt;

    return Row(
      children: [
        const Icon(Icons.access_time, size: 16),
        const SizedBox(width: 10),
        Text(
          '${AppLocalizations.of(context)!.lastUpdated} ${updatedAt.month}/${updatedAt.year}',
        ),
      ],
    );
  }

  Widget _locale(BuildContext context) {
    return Row(
      children: [
        const Icon(Icons.language, size: 16),
        const SizedBox(width: 10),
        Text(course.locale.englishTitle),
      ],
    );
  }

  Widget _tagsList(BuildContext context) {
    return Wrap(
      spacing: 5,
      runSpacing: 5,
      children: course.tags.map((tag) {
        return Container(
          padding: const EdgeInsets.symmetric(vertical: 5, horizontal: 10),
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(9999),
            color: Theme.of(context).primaryColor.withOpacity(0.7),
          ),
          child: Text(tag),
        );
      }).toList(),
    );
  }
}
