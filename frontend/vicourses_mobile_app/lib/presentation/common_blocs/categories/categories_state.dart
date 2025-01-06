part of 'categories_cubit.dart';

class CategoriesState {
  final bool isLoading;
  final List<Category>? categories;

  CategoriesState({
    this.isLoading = false,
    this.categories,
  });
}
