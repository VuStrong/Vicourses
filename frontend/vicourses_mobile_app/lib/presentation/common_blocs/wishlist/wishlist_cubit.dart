import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/models/wishlist.dart';
import 'package:vicourses_mobile_app/services/api/wishlist_service.dart';

part 'wishlist_state.dart';

class WishlistCubit extends Cubit<WishlistState> {
  final WishlistService _wishlistService;

  WishlistCubit(this._wishlistService) : super(WishlistState());

  Future<void> getWishlist() async {
    emit(WishlistState(isLoading: true));

    final wishlist = await _wishlistService.getWishlist();

    if (wishlist != null) {
      final courseIds = wishlist.courses.map((course) => course.id).toSet();

      emit(WishlistState(
        isLoading: false,
        wishlist: wishlist,
        courseIds: courseIds,
      ));

      return;
    }

    emit(WishlistState(isLoading: false));
  }

  Future<void> refresh() async {
    if (state.isLoading || state.isRefreshing) return;

    emit(state.copyWith(isRefreshing: true));

    final wishlist = await _wishlistService.getWishlist();

    if (wishlist != null) {
      final courseIds = wishlist.courses.map((course) => course.id).toSet();

      emit(state.copyWith(
        isRefreshing: false,
        wishlist: () => wishlist,
        courseIds: courseIds,
      ));
      return;
    }

    emit(state.copyWith(isRefreshing: false));
  }

  Future<void> addToWishlist(String courseId) async {
    state.courseIds.add(courseId);
    emit(state.copyWith());

    try {
      final wishlist = await _wishlistService.addToWishlist(courseId);

      emit(state.copyWith(
        wishlist: () => wishlist,
      ));
    } on Exception {
      state.courseIds.remove(courseId);
      emit(state.copyWith());
    }
  }

  Future<void> removeFromWishlist(String courseId) async {
    state.courseIds.remove(courseId);
    emit(state.copyWith());

    try {
      final wishlist = await _wishlistService.removeFromWishlist(courseId);

      emit(state.copyWith(
        wishlist: () => wishlist,
      ));
    } on Exception {
      state.courseIds.add(courseId);
      emit(state.copyWith());
    }
  }
}
