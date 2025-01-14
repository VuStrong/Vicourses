import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:pull_to_refresh/pull_to_refresh.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/cubit/course_ratings.dart';
import 'package:vicourses_mobile_app/presentation/screens/course_detail/widgets/rating_item.dart';
import 'package:vicourses_mobile_app/services/api/rating_service.dart';

class CourseRatingsScreen extends StatefulWidget {
  final String courseId;

  const CourseRatingsScreen({
    super.key,
    required this.courseId,
  });

  @override
  State<CourseRatingsScreen> createState() => _CourseRatingsScreenState();
}

class _CourseRatingsScreenState extends State<CourseRatingsScreen> {
  final List<int> stars = [1, 2, 3, 4, 5];

  late RefreshController _refreshController;

  @override
  void initState() {
    _refreshController = RefreshController();

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider<CourseRatingsCubit>(
      create: (_) =>
          CourseRatingsCubit(RatingService())..fetchRatings(widget.courseId),
      child: Scaffold(
        appBar: AppBar(
          title: Text(AppLocalizations.of(context)!.rating),
        ),
        body: Column(
          children: [
            _starFilter(),
            const Divider(),
            Expanded(
              child: _ratingsList(),
            ),
          ],
        ),
      ),
    );
  }

  void _onTapStar(BuildContext context, int? star) {
    context.read<CourseRatingsCubit>().fetchRatings(
          widget.courseId,
          starFilter: star,
        );
  }

  Widget _starFilter() {
    return BlocBuilder<CourseRatingsCubit, CourseRatingsState>(
      buildWhen: (prev, current) => prev.starFilter != current.starFilter,
      builder: (context, state) {
        return SizedBox(
          height: 40,
          child: ListView(
            shrinkWrap: true,
            scrollDirection: Axis.horizontal,
            children: stars.map((star) {
              final button = star == state.starFilter
                  ? OutlinedButton.icon(
                      onPressed: () => _onTapStar(context, null),
                      icon: const Icon(Icons.check),
                      label: Text('$star ⭐️'),
                    )
                  : OutlinedButton(
                      onPressed: () => _onTapStar(context, star),
                      child: Text('$star ⭐️'),
                    );

              return Padding(
                padding: const EdgeInsets.only(right: 5),
                child: button,
              );
            }).toList(),
          ),
        );
      },
    );
  }

  Widget _ratingsList() {
    return BlocConsumer<CourseRatingsCubit, CourseRatingsState>(
      listenWhen: (prev, current) =>
          (prev.isLoadingMore && !current.isLoadingMore) ||
          prev.ratings != current.ratings,
      listener: (context, state) {
        if (state.end) {
          _refreshController.loadNoData();
        } else {
          _refreshController.loadComplete();
        }
      },
      buildWhen: (prev, current) =>
          prev.isLoading != current.isLoading ||
          prev.ratings != current.ratings,
      builder: (context, state) {
        if (state.isLoading) {
          return const Center(
            child: CircularProgressIndicator(),
          );
        }

        if (state.ratings.isEmpty) {
          return Center(
            child: Text(AppLocalizations.of(context)!.noResults),
          );
        }

        return SmartRefresher(
          enablePullUp: true,
          enablePullDown: false,
          controller: _refreshController,
          onLoading: () {
            context.read<CourseRatingsCubit>().loadMore();
          },
          child: SingleChildScrollView(
            padding: const EdgeInsets.symmetric(horizontal: 10),
            child: Column(
              children: state.ratings
                  .map((rating) => Padding(
                        padding: const EdgeInsets.symmetric(vertical: 10),
                        child: RatingItem(rating: rating),
                      ))
                  .toList(),
            ),
          ),
        );
      },
    );
  }
}
