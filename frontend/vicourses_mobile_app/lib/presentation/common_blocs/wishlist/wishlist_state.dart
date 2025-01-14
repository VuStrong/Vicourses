part of 'wishlist_cubit.dart';

class WishlistState {
  final bool isLoading;
  final bool isRefreshing;
  final Wishlist? wishlist;
  Set<String> courseIds;

  WishlistState({
    this.isLoading = false,
    this.isRefreshing = false,
    this.wishlist,
    this.courseIds = const {},
  });

  WishlistState copyWith({
    bool? isLoading,
    bool? isRefreshing,
    Wishlist? Function()? wishlist,
    Set<String>? courseIds,
  }) {
    return WishlistState(
      isLoading: isLoading ?? this.isLoading,
      isRefreshing: isRefreshing ?? this.isRefreshing,
      wishlist: wishlist != null ? wishlist() : this.wishlist,
      courseIds: courseIds ?? this.courseIds,
    );
  }
}
