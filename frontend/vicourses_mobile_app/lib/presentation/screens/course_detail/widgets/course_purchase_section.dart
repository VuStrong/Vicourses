import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/add_to_wishlist_button.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/course_detail.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/enroll_button.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/pay_button.dart';

class CoursePurchaseSection extends StatelessWidget {
  final CourseDetail course;

  const CoursePurchaseSection({
    super.key,
    required this.course,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const SizedBox(height: 20),
        Text(
          course.isPaid
              ? '\$${course.price.toString()}'
              : AppLocalizations.of(context)!.free,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 22,
          ),
        ),
        _actionButtons(),
        const SizedBox(height: 20),
      ],
    );
  }

  Widget _actionButtons() {
    return BlocBuilder<CourseDetailCubit, CourseDetailState>(
      buildWhen: (prev, current) =>
          prev.isCheckingEnroll != current.isCheckingEnroll ||
          prev.enrolled != current.enrolled,
      builder: (context, state) {
        if (state.isCheckingEnroll) {
          return const Center(child: CircularProgressIndicator());
        }

        if (state.enrolled) {
          return Row(
            children: [Expanded(child: _enrolledLabel(context))],
          );
        }

        if (course.isPaid) {
          return Row(
            children: [
              Expanded(child: PayButton(courseId: course.id)),
              AddToWishlistButton(courseId: course.id),
            ],
          );
        }

        return const Row(
          children: [
            Expanded(child: EnrollButton()),
          ],
        );
      },
    );
  }

  Widget _enrolledLabel(BuildContext context) {
    return ElevatedButton.icon(
      style: ElevatedButton.styleFrom(
        backgroundColor: Colors.black,
        shape: const RoundedRectangleBorder(),
      ),
      onPressed: () {},
      icon: const Icon(Icons.info_outline, color: Colors.white),
      label: Text(
        AppLocalizations.of(context)!.youAreEnrolled,
        style: const TextStyle(color: Colors.white),
      ),
    );
  }
}
