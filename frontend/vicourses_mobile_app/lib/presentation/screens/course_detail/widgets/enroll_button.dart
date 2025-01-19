import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/course_detail.dart';

class EnrollButton extends StatelessWidget {
  const EnrollButton({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<CourseDetailCubit, CourseDetailState>(
      buildWhen: (prev, current) =>
          prev.enrollingStatus != current.enrollingStatus,
      builder: (context, state) {
        return ElevatedButton(
          style: ElevatedButton.styleFrom(
            backgroundColor: Colors.black,
            shape: const RoundedRectangleBorder(),
          ),
          onPressed: () {
            if (state.enrollingStatus == EnrollingStatus.pending) return;

            context.read<CourseDetailCubit>().enroll();
          },
          child: state.enrollingStatus != EnrollingStatus.pending
              ? Text(
                  AppLocalizations.of(context)!.enrollNow,
                  style: const TextStyle(color: Colors.white),
                )
              : const CircularProgressIndicator(),
        );
      },
    );
  }
}
