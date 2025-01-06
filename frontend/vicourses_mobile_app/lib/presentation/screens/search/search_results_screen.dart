import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:pull_to_refresh/pull_to_refresh.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/course/course_item.dart';
import 'package:vicourses_mobile_app/presentation/screens/search/cubit/search.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

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

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    _refreshController = RefreshController();

    return BlocProvider<SearchCubit>(
      create: (_) => SearchCubit(CourseService())
        ..search(
          searchValue: widget.searchValue,
        ),
      child: Scaffold(
        appBar: AppBar(
          automaticallyImplyLeading: false,
          title: _searchBar(),
          actions: [
            IconButton(
              onPressed: () {},
              icon: const Icon(Icons.filter_alt),
            )
          ],
        ),
        body: BlocConsumer<SearchCubit, SearchState>(
          listener: (context, state) {
            if (state.isLoading) {
              return;
            }

            if (state.end) {
              _refreshController.loadNoData();
            } else {
              _refreshController.loadComplete();
            }
          },
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
        child: Column(
          children: state.courses!
              .map((course) => CourseItem(
                    course: course,
                    onTap: () {},
                  ))
              .toList(),
        ),
      ),
    );
  }
}
