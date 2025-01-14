import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:pull_to_refresh/pull_to_refresh.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/categories/categories.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/wishlist/wishlist.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/course/wishlist_course_item.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class WishlistScreen extends StatefulWidget {
  const WishlistScreen({super.key});

  @override
  State<WishlistScreen> createState() => _WishlistScreenState();
}

class _WishlistScreenState extends State<WishlistScreen> {
  late RefreshController _refreshController;

  @override
  void initState() {
    _refreshController = RefreshController();

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(AppLocalizations.of(context)!.wishlist),
      ),
      body: BlocListener<WishlistCubit, WishlistState>(
        listenWhen: (prev, current) =>
            prev.isRefreshing && !current.isRefreshing,
        listener: (_, __) {
          _refreshController.refreshCompleted();
        },
        child: SmartRefresher(
          controller: _refreshController,
          enablePullUp: false,
          enablePullDown: true,
          onRefresh: () {
            context.read<WishlistCubit>().refresh();
          },
          child: ListView(
            padding: const EdgeInsets.all(10),
            children: [
              _wishlistSection(),
              const SizedBox(height: 100),
              _categoriesSection(context),
            ],
          ),
        ),
      ),
    );
  }

  Widget _wishlistSection() {
    return BlocBuilder<WishlistCubit, WishlistState>(
      buildWhen: (prev, current) => prev.wishlist != current.wishlist,
      builder: (context, state) {
        if (state.wishlist == null || state.wishlist!.count == 0) {
          return _emptyWishlist(context);
        }

        return _courseList(context, state);
      },
    );
  }

  Widget _emptyWishlist(BuildContext context) {
    return Column(
      children: [
        AspectRatio(
          aspectRatio: 16 / 9,
          child: Image.asset(
            AppConstants.defaultCourseImagePath,
            fit: BoxFit.cover,
          ),
        ),
        const SizedBox(height: 20),
        Text(
          AppLocalizations.of(context)!.emptyWishlist,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            color: Colors.grey,
          ),
        ),
      ],
    );
  }

  Widget _courseList(BuildContext context, WishlistState state) {
    return Column(
      children: state.wishlist!.courses
          .map((course) => WishlistCourseItem(course: course))
          .toList(),
    );
  }

  Widget _categoriesSection(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        const SizedBox(height: 20),
        Text(
          AppLocalizations.of(context)!.browseCategories,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 16,
          ),
        ),
        BlocBuilder<CategoriesCubit, CategoriesState>(
          builder: (context, state) {
            if (state.isLoading || state.categories == null) {
              return const CircularProgressIndicator();
            }

            return Column(
              children: state.categories!
                  .map(
                    (category) => ListTile(
                      title: Text(
                        category.name,
                        style: const TextStyle(fontSize: 14),
                      ),
                      contentPadding: EdgeInsets.zero,
                      onTap: () {
                        context.push(AppRoutes.getCategoryRoute(category.slug));
                      },
                    ),
                  )
                  .toList(),
            );
          },
        ),
      ],
    );
  }
}
