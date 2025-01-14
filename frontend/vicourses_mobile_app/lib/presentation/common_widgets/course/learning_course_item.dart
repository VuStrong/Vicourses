import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class LearningCourseItem extends StatelessWidget {
  final Course course;

  const LearningCourseItem({
    super.key,
    required this.course,
  });

  @override
  Widget build(BuildContext context) {
    return ListTile(
      contentPadding: EdgeInsets.zero,
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
        ],
      ),
      shape: Border(
        bottom: BorderSide(
          color: Colors.grey.withOpacity(0.3),
        ),
      ),
      onTap: () {
        context.push(AppRoutes.getLearningRoute(course.id));
      },
    );
  }
}
