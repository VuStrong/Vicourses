import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/category.dart';
import 'package:vicourses_mobile_app/services/api/category_service.dart';

part 'categories_state.dart';

class CategoriesCubit extends Cubit<CategoriesState> {
  final CategoryService _categoryService;

  CategoriesCubit(this._categoryService) : super(CategoriesState());

  Future<void> getCategories() async {
    emit(CategoriesState(isLoading: true));

    final categories = await _categoryService.getCategories();

    emit(CategoriesState(
      isLoading: false,
      categories: categories,
    ));
  }
}
