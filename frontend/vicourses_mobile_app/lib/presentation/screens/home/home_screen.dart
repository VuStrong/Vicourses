import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/category.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/categories/categories.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/course/course_item.dart';
import 'package:vicourses_mobile_app/presentation/screens/home/cubit/home.dart';
import 'package:vicourses_mobile_app/routes/app_routes.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return BlocProvider<HomeCubit>(
      create: (_) => HomeCubit(CourseService())..fetchHomeContent(),
      child: Scaffold(
        appBar: AppBar(
          title: Text(AppLocalizations.of(context)!.home),
        ),
        body: ListView(
          children: [
            _banner(),
            const SizedBox(height: 30),
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: 10),
              child: _categoriesSection(context),
            ),
            Padding(
              padding: const EdgeInsets.symmetric(
                horizontal: 10,
                vertical: 30,
              ),
              child: _newestCoursesSection(),
            ),
            const SizedBox(height: 50),
          ],
        ),
      ),
    );
  }

  Widget _banner() {
    return AspectRatio(
      aspectRatio: 16 / 9,
      child: Image.network(
        'https://res.cloudinary.com/dsrcm9jcs/image/upload/v1734940749/Others/banner1.jpg',
        fit: BoxFit.cover,
      ),
    );
  }

  Widget _categoriesSection(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.browseCategories,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 22,
          ),
        ),
        const SizedBox(height: 10),
        BlocBuilder<CategoriesCubit, CategoriesState>(
          builder: (context, state) {
            if (state.categories == null) return const SizedBox();

            final length = state.categories!.length;
            final categoriesList1 = state.categories!.take((length / 2).ceil());
            final categoriesList2 =
                state.categories!.sublist((length / 2).ceil());

            return SizedBox(
              height: 100,
              child: SingleChildScrollView(
                scrollDirection: Axis.horizontal,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      children: categoriesList1.map((category) {
                        return Padding(
                          padding: const EdgeInsets.only(right: 10),
                          child: _categoryButton(context, category),
                        );
                      }).toList(),
                    ),
                    Row(
                      children: categoriesList2.map((category) {
                        return Padding(
                          padding: const EdgeInsets.only(right: 10),
                          child: _categoryButton(context, category),
                        );
                      }).toList(),
                    ),
                  ],
                ),
              ),
            );
          },
        ),
      ],
    );
  }

  Widget _categoryButton(BuildContext context, Category category) {
    return OutlinedButton(
      onPressed: () {
        context.push(AppRoutes.getCategoryRoute(category.slug));
      },
      child: Text(
        category.name,
        style: const TextStyle(color: Colors.black),
      ),
    );
  }

  Widget _newestCoursesSection() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(
          AppLocalizations.of(context)!.newestCourses,
          style: const TextStyle(
            fontWeight: FontWeight.bold,
            fontSize: 22,
          ),
        ),
        const SizedBox(height: 10),
        BlocBuilder<HomeCubit, HomeState>(
          builder: (context, state) {
            if (state.isLoading) {
              return const Center(child: CircularProgressIndicator());
            }

            if (state.courses == null) return const SizedBox();

            return SizedBox(
              height: 240,
              child: ListView(
                scrollDirection: Axis.horizontal,
                shrinkWrap: true,
                children: state.courses!.map((course) {
                  return Container(
                    width: 240,
                    padding: const EdgeInsets.only(right: 20),
                    child: CourseItem(
                      course: course,
                      style: CourseItemStyle.card,
                    ),
                  );
                }).toList(),
              ),
            );
          },
        )
      ],
    );
  }
}
