import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/star_rating.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

enum CourseItemStyle {
  card,
  tile,
}

class CourseItem extends StatelessWidget {
  final Course course;
  final CourseItemStyle style;
  final void Function()? onTap;

  const CourseItem({
    super.key,
    required this.course,
    this.style = CourseItemStyle.tile,
    this.onTap,
  });

  @override
  Widget build(BuildContext context) {
    if (style == CourseItemStyle.tile) {
      return _tileStyle(context);
    }

    return _cardStyle(context);
  }

  Widget _tileStyle(BuildContext context) {
    return ListTile(
      isThreeLine: true,
      leading: SizedBox(
        width: 56,
        height: 56,
        child: ClipRRect(
          borderRadius: BorderRadius.circular(10),
          child: course.thumbnailUrl != null
              ? Image.network(
                  course.thumbnailUrl!,
                  fit: BoxFit.cover,
                )
              : Image.asset(
                  AppConstants.defaultCourseImagePath,
                  fit: BoxFit.cover,
                ),
        ),
      ),
      title: Text(
        course.title,
        style: const TextStyle(
          fontWeight: FontWeight.bold,
          fontSize: 14,
        ),
      ),
      subtitle: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            course.user.name,
            style: const TextStyle(fontSize: 13),
          ),
          Row(
            children: [
              Text(
                course.rating.toString(),
                style: const TextStyle(
                  color: Colors.orangeAccent,
                  fontWeight: FontWeight.w600,
                ),
              ),
              const SizedBox(width: 4),
              StarRating(
                initialRating: course.rating.toDouble(),
                readonly: true,
                itemSize: 18,
              ),
            ],
          ),
          Text(
            course.isPaid
                ? '\$ ${course.price}'
                : AppLocalizations.of(context)!.free,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 16,
            ),
          )
        ],
      ),
      shape: Border(
        bottom: BorderSide(
          color: Colors.grey.withOpacity(0.3),
        ),
      ),
      onTap: onTap,
    );
  }

  Widget _cardStyle(BuildContext context) {
    return const ListTile();
  }
}
