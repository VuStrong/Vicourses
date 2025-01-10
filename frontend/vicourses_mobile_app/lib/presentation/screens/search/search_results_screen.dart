import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:pull_to_refresh/pull_to_refresh.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/course/course_item.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/course_sort.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/cubit/search.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class SearchResultsScreen extends StatefulWidget {
  final String searchValue;

  const SearchResultsScreen({
    super.key,
    required this.searchValue,
  });

  @override
  State<SearchResultsScreen> createState() => _SearchResultsScreenState();
}

class _SearchResultsScreenState extends State<SearchResultsScreen> {
  final TextEditingController _searchController = TextEditingController();
  late RefreshController _refreshController;

  @override
  void initState() {
    _searchController.text = widget.searchValue;
    _refreshController = RefreshController();

    super.initState();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider<SearchCubit>(
      create: (_) =>
          SearchCubit(CourseService())..search(searchValue: widget.searchValue),
      child: Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          title: _searchBar(),
          actions: [
            Builder(builder: (context) {
              return IconButton(
                onPressed: () {
                  Scaffold.of(context).openDrawer();
                },
                icon: const Icon(Icons.filter_alt),
              );
            })
          ],
        ),
        drawer: const _FilterDrawer(),
        body: BlocConsumer<SearchCubit, SearchState>(
          listenWhen: (prev, current) =>
              prev.isLoadingMore && !current.isLoadingMore ||
              prev.courses != current.courses,
          listener: (context, state) {
            if (state.end) {
              _refreshController.loadNoData();
            } else {
              _refreshController.loadComplete();
            }
          },
          buildWhen: (prev, current) =>
              prev.isLoading != current.isLoading ||
              prev.courses != current.courses,
          builder: (context, state) {
            if (state.isLoading) {
              return const Center(
                child: CircularProgressIndicator(),
              );
            }

            if (state.courses == null || state.courses!.isEmpty) {
              return Center(
                child: Text(AppLocalizations.of(context)!.noResults),
              );
            }

            return _coursesList(context, state);
          },
        ),
      ),
    );
  }

  Widget _searchBar() {
    return BlocBuilder<SearchCubit, SearchState>(
      buildWhen: (_, __) => false,
      builder: (context, state) {
        return TextField(
          controller: _searchController,
          decoration: InputDecoration(
            icon: const Icon(Icons.search_outlined),
            hintText: AppLocalizations.of(context)!.search,
            border: InputBorder.none,
          ),
          onSubmitted: (value) {
            if (value.isEmpty) return;

            context.read<SearchCubit>().search(searchValue: value);
          },
        );
      },
    );
  }

  Widget _coursesList(BuildContext context, SearchState state) {
    return SmartRefresher(
      enablePullUp: true,
      enablePullDown: false,
      footer: CustomFooter(
        builder: (BuildContext context, LoadStatus? mode) {
          late final Widget body;

          if (mode == LoadStatus.loading) {
            body = const CircularProgressIndicator();
          } else {
            body = const SizedBox.shrink();
          }

          return SizedBox(
            height: 55,
            child: Center(child: body),
          );
        },
      ),
      controller: _refreshController,
      onLoading: () {
        context.read<SearchCubit>().loadMore();
      },
      child: SingleChildScrollView(
        padding: const EdgeInsets.symmetric(horizontal: 10),
        child: Column(
          children: state.courses!
              .map((course) => CourseItem(course: course))
              .toList(),
        ),
      ),
    );
  }
}

class _FilterDrawer extends StatelessWidget {
  const _FilterDrawer();

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: BlocBuilder<SearchCubit, SearchState>(
        buildWhen: (prev, current) =>
            prev.rating != current.rating ||
            prev.free != current.free ||
            prev.level != current.level ||
            prev.sort != current.sort ||
            prev.total != current.total,
        builder: (context, state) {
          return Column(
            children: [
              Expanded(
                child: ListView(
                  padding: const EdgeInsets.all(10),
                  children: [
                    Text(
                      '${state.total} ${AppLocalizations.of(context)!.results}',
                      style: const TextStyle(
                        fontWeight: FontWeight.bold,
                        fontSize: 20,
                      ),
                    ),
                    const SizedBox(height: 30),
                    _dropdownSortButton(context, state),
                    _ratingFilter(context, state),
                    _freeFilter(context, state),
                    _levelFilter(context, state),
                  ],
                ),
              ),
              const Divider(),
              Row(
                children: [
                  Expanded(
                    child: _resetFilterButton(context),
                  ),
                  Expanded(
                    child: _applyButton(context),
                  ),
                ],
              ),
            ],
          );
        },
      ),
    );
  }

  Widget _dropdownSortButton(BuildContext context, SearchState state) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 10),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            AppLocalizations.of(context)!.sort,
            style: const TextStyle(fontWeight: FontWeight.bold),
          ),
          DropdownButton<String>(
            value: state.sort,
            icon: const Icon(Icons.arrow_downward),
            elevation: 16,
            underline: Container(height: 2),
            onChanged: (String? value) {
              context
                  .read<SearchCubit>()
                  .setSort(value ?? CourseSort.relevance);
            },
            items: const [
              DropdownMenuItem<String>(
                value: CourseSort.relevance,
                child: Text(CourseSort.relevance),
              ),
              DropdownMenuItem<String>(
                value: CourseSort.highestRated,
                child: Text(CourseSort.highestRated),
              ),
              DropdownMenuItem<String>(
                value: CourseSort.newest,
                child: Text(CourseSort.newest),
              ),
            ],
          )
        ],
      ),
    );
  }

  Widget _ratingFilter(BuildContext context, SearchState state) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 10),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            AppLocalizations.of(context)!.rating,
            style: const TextStyle(fontWeight: FontWeight.bold),
          ),
          CheckboxListTile(
            title: Text(AppLocalizations.of(context)!.fromNumAndAbove(4)),
            value: state.rating == 4,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyRatingFilter(value == true ? 4 : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
          CheckboxListTile(
            title: Text(AppLocalizations.of(context)!.fromNumAndAbove(3)),
            value: state.rating == 3,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyRatingFilter(value == true ? 3 : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
          CheckboxListTile(
            title: Text(AppLocalizations.of(context)!.fromNumAndAbove(2)),
            value: state.rating == 2,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyRatingFilter(value == true ? 2 : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
        ],
      ),
    );
  }

  Widget _freeFilter(BuildContext context, SearchState state) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 10),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            AppLocalizations.of(context)!.price,
            style: const TextStyle(fontWeight: FontWeight.bold),
          ),
          CheckboxListTile(
            title: Text(AppLocalizations.of(context)!.paid),
            value: state.free == false,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyFreeFilter(value == true ? false : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
          CheckboxListTile(
            title: Text(AppLocalizations.of(context)!.free),
            value: state.free == true,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyFreeFilter(value == true ? true : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
        ],
      ),
    );
  }

  Widget _levelFilter(BuildContext context, SearchState state) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 10),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            AppLocalizations.of(context)!.level,
            style: const TextStyle(fontWeight: FontWeight.bold),
          ),
          CheckboxListTile(
            title: const Text(CourseLevel.all),
            value: state.level == CourseLevel.all,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyLevelFilter(value == true ? CourseLevel.all : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
          CheckboxListTile(
            title: const Text(CourseLevel.basic),
            value: state.level == CourseLevel.basic,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyLevelFilter(value == true ? CourseLevel.basic : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
          CheckboxListTile(
            title: const Text(CourseLevel.intermediate),
            value: state.level == CourseLevel.intermediate,
            onChanged: (bool? value) {
              context.read<SearchCubit>().applyLevelFilter(
                  value == true ? CourseLevel.intermediate : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
          CheckboxListTile(
            title: const Text(CourseLevel.expert),
            value: state.level == CourseLevel.expert,
            onChanged: (bool? value) {
              context
                  .read<SearchCubit>()
                  .applyLevelFilter(value == true ? CourseLevel.expert : null);
            },
            contentPadding: EdgeInsets.zero,
          ),
        ],
      ),
    );
  }

  Widget _resetFilterButton(BuildContext context) {
    return TextButton(
      onPressed: () {
        context.read<SearchCubit>().resetFilter();
      },
      child: Text(AppLocalizations.of(context)!.reset),
    );
  }

  Widget _applyButton(BuildContext context) {
    return TextButton(
      onPressed: () {
        context.read<SearchCubit>().search();

        Navigator.pop(context);
      },
      child: Text(AppLocalizations.of(context)!.apply),
    );
  }
}
