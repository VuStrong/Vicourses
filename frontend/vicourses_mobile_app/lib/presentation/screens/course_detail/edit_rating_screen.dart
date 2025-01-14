import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:loader_overlay/loader_overlay.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/snack_bar.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/star_rating.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/user_rating.dart';

class EditRatingScreen extends StatefulWidget {
  final String courseId;
  final UserRatingCubit cubit;

  const EditRatingScreen({
    super.key,
    required this.courseId,
    required this.cubit,
  });

  @override
  State<EditRatingScreen> createState() => _EditRatingScreenState();
}

class _EditRatingScreenState extends State<EditRatingScreen> {
  final _formKey = GlobalKey<FormState>();
  late final TextEditingController _feedbackController;
  late double _star;

  @override
  void initState() {
    final rating = widget.cubit.state.rating;
    _feedbackController = TextEditingController(text: rating?.feedback ?? '');
    _star = rating?.star.toDouble() ?? 1;
    super.initState();
  }

  @override
  void dispose() {
    _feedbackController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider.value(
      value: widget.cubit,
      child: Scaffold(
        appBar: AppBar(
          leading: IconButton(
            onPressed: () => context.pop(),
            icon: const Icon(Icons.close),
          ),
          title: Text(AppLocalizations.of(context)!.writeFeedback),
          actions: [_saveButton()],
        ),
        body: BlocListener<UserRatingCubit, UserRatingState>(
          listenWhen: (prev, current) => prev.editStatus != current.editStatus,
          listener: (context, state) {
            if (state.editStatus == EditRatingStatus.pending) {
              return context.loaderOverlay.show();
            }

            context.loaderOverlay.hide();

            if (state.editStatus == EditRatingStatus.failed) {
              return showSnackBar(
                context: context,
                text: AppLocalizations.of(context)!.errorOccurred,
                type: SnackBarType.error,
              );
            }

            if (state.editStatus == EditRatingStatus.success) {
              context.pop();
            }
          },
          child: _ratingForm(context),
        ),
      ),
    );
  }

  Widget _ratingForm(BuildContext context) {
    return Form(
      key: _formKey,
      child: ListView(
        padding: const EdgeInsets.all(10),
        children: [
          _ratingBar(),
          const SizedBox(height: 20),
          _feedbackField(context),
        ],
      ),
    );
  }

  Widget _ratingBar() {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: [
        StarRating(
          initialRating: _star,
          minRating: 1,
          onRatingUpdate: (star) {
            setState(() {
              _star = star;
            });
          },
          itemSize: 50,
          allowHalfRating: false,
        ),
      ],
    );
  }

  Widget _feedbackField(BuildContext context) {
    return TextFormField(
      controller: _feedbackController,
      keyboardType: TextInputType.multiline,
      maxLines: null,
      decoration: InputDecoration(
        hintText: AppLocalizations.of(context)!.writeFeedback,
      ),
      onTapOutside: (event) => FocusScope.of(context).unfocus(),
      validator: (value) {
        final msg = AppLocalizations.of(context)!.textLength(1, 400);

        if (value?.isEmpty ?? true) return msg;

        return value!.length <= 400 ? null : msg;
      },
    );
  }

  Widget _saveButton() {
    return BlocBuilder<UserRatingCubit, UserRatingState>(
      builder: (context, state) {
        return TextButton(
          onPressed: () {
            if (_formKey.currentState?.validate() ?? false) {
              context.read<UserRatingCubit>().editRating(
                courseId: widget.courseId,
                feedback: _feedbackController.text,
                star: _star.toInt(),
              );
            }
          },
          child: Text(AppLocalizations.of(context)!.save),
        );
      },
    );
  }
}
