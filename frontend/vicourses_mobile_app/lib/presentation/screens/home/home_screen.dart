import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:go_router/go_router.dart';
import 'package:vicourses_mobile_app/models/category.dart';
import 'package:vicourses_mobile_app/models/course.dart';
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
              padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
              child: _categoriesSection(context),
            ),
            _coursesSectionsBuilder(),
            const SizedBox(height: 50),
          ],
        ),
      ),
    );
  }

  Widget _banner() {
    return Column(
      children: [
        AspectRatio(
          aspectRatio: 16 / 9,
          child: Image.network(
            'https://res.cloudinary.com/dsrcm9jcs/image/upload/v1734940749/Others/banner1.jpg',
            fit: BoxFit.cover,
          ),
        ),
        const Padding(
          padding: EdgeInsets.symmetric(horizontal: 20),
          child: Text(
            'Study what you are interested in',
            style: TextStyle(
              fontSize: 22,
              fontWeight: FontWeight.bold,
            ),
            textAlign: TextAlign.center,
          ),
        ),
        const Padding(
          padding: EdgeInsets.symmetric(horizontal: 20),
          child: Text(
            'Skills for your present (and your future). Start studying with us.',
            textAlign: TextAlign.center,
          ),
        ),
      ],
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

  Widget _coursesSectionsBuilder() {
    return BlocBuilder<HomeCubit, HomeState>(
      builder: (context, state) {
        return Column(
          children: [
            _CoursesSection(
              title: AppLocalizations.of(context)!.newestCourses,
              isLoading: state.isLoading,
              courses: state.newestCourses,
            ),
            _CoursesSection(
              title: AppLocalizations.of(context)!.highestRated,
              isLoading: state.isLoading,
              courses: state.highestRatedCourses,
            ),
          ],
        );
      },
    );
  }
}

class _CoursesSection extends StatelessWidget {
  final String title;
  final List<Course>? courses;
  final bool isLoading;

  const _CoursesSection({
    required this.title,
    this.courses,
    this.isLoading = false,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(
        horizontal: 10,
        vertical: 20,
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            title,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 22,
            ),
          ),
          const SizedBox(height: 10),
          if (isLoading) const Center(child: CircularProgressIndicator()),
          if (!isLoading && courses != null)
            SizedBox(
              height: 240,
              child: ListView.builder(
                scrollDirection: Axis.horizontal,
                shrinkWrap: true,
                itemCount: courses!.length,
                itemBuilder: (context, index) {
                  return Container(
                    width: 240,
                    padding: const EdgeInsets.only(right: 20),
                    child: CourseItem(
                      course: courses![index],
                      style: CourseItemStyle.card,
                    ),
                  );
                },
              ),
            ),
        ],
      ),
    );
  }
}
