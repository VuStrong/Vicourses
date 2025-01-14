import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/category.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/screens/category/course_sort.dart';
import 'package:vicourses_mobile_app/services/api/category_service.dart';
import 'package:vicourses_mobile_app/services/api/course_service.dart';

part 'category_state.dart';

class CategoryCubit extends Cubit<CategoryState> {
  final int limit;
  final CourseService _courseService;
  final CategoryService _categoryService;

  CategoryCubit(
    this._courseService,
    this._categoryService, {
    this.limit = 15,
  }) : super(CategoryState());

  Future<void> fetchCategory(String slug) async {
    emit(CategoryState(isLoading: true));

    final category = await _categoryService.getCategory(slug);

    emit(CategoryState(category: category, isLoading: false));

    fetchCourses();
  }

  Future<void> fetchCourses() async {
    if (state.category == null || state.isLoading || state.isLoadingMore) {
      return;
    }

    emit(state.copyWith(isLoading: true));

    final category = state.category!;
    final result = await _courseService.getCourses(
      limit: limit,
      categoryId: category.isRoot ? category.id : null,
      subCategoryId: category.isRoot ? null : category.id,
      sort: state.sort,
      rating: state.rating,
      level: state.level,
      free: state.free,
    );

    if (result != null) {
      emit(state.copyWith(
        isLoading: false,
        courses: () => result.items,
        end: result.end,
        total: result.total,
      ));
    }
  }

  void applyRatingFilter(num? rating) {
    emit(state.copyWith(rating: () => rating));
  }

  void applyLevelFilter(String? level) {
    emit(state.copyWith(level: () => level));
  }

  void applyFreeFilter(bool? free) {
    emit(state.copyWith(free: () => free));
  }

  void resetFilter() {
    emit(state.copyWith(
      rating: () => null,
      level: () => null,
      free: () => null,
    ));
  }

  void setSort(String sort) {
    emit(state.copyWith(sort: sort));
  }

  Future<void> loadMore() async {
    if (state.category == null || state.isLoading || state.isLoadingMore) {
      return;
    }

    emit(state.copyWith(isLoadingMore: true));

    final skip = state.courses != null ? state.courses!.length : 0;
    final category = state.category!;

    final result = await _courseService.getCourses(
      skip: skip,
      limit: limit,
      categoryId: category.isRoot ? category.id : null,
      subCategoryId: category.isRoot ? null : category.id,
      sort: state.sort,
      rating: state.rating,
      level: state.level,
      free: state.free,
    );

    if (result != null) {
      final oldCourses = state.courses ?? [];
      final newCourses = [...oldCourses, ...result.items];

      emit(state.copyWith(
        courses: () => newCourses,
        end: result.end,
        isLoadingMore: false,
      ));
    }
  }
}
