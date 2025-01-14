import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/categories/categories.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';

class SearchScreen extends StatefulWidget {
  const SearchScreen({super.key});

  @override
  State<SearchScreen> createState() => _SearchScreenState();
}

class _SearchScreenState extends State<SearchScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: TextField(
          decoration: InputDecoration(
            icon: const Icon(Icons.search_outlined),
            hintText: AppLocalizations.of(context)!.search,
            border: InputBorder.none,
          ),
          onSubmitted: (value) {
            if (value.isEmpty) return;

            context.push('${AppRoutes.searchResults}?q=$value');
          },
        ),
      ),
      body: _listCategories(context),
    );
  }

  Widget _listCategories(BuildContext context) {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const SizedBox(height: 20),
          Padding(
            padding: const EdgeInsets.only(left: 14, right: 14),
            child: Text(
              AppLocalizations.of(context)!.browseCategories,
              style: const TextStyle(
                fontWeight: FontWeight.bold,
                fontSize: 16,
              ),
            ),
          ),
          BlocBuilder<CategoriesCubit, CategoriesState>(
            builder: (context, state) {
              if (state.isLoading || state.categories == null) {
                return const CircularProgressIndicator();
              }

              return Column(
                children: state.categories!
                    .map((category) => ListTile(
                          title: Text(
                            category.name,
                            style: const TextStyle(fontSize: 14),
                          ),
                          contentPadding:
                              const EdgeInsets.symmetric(horizontal: 14),
                          onTap: () {
                            context.push(
                                AppRoutes.getCategoryRoute(category.slug));
                          },
                        ))
                    .toList(),
              );
            },
          ),
        ],
      ),
    );
  }
}
