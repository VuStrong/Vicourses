import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:vicourses_mobile_app/presentation/common_blocs/wishlist/wishlist.dart';

class AddToWishlistButton extends StatelessWidget {
  final String courseId;
  final double size;

  const AddToWishlistButton({
    super.key,
    required this.courseId,
    this.size = 50,
  });

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<WishlistCubit, WishlistState>(
      builder: (context, state) {
        final added = state.courseIds.contains(courseId);

        return IconButton(
          onPressed: () {
            if (added) {
              context.read<WishlistCubit>().removeFromWishlist(courseId);
            } else {
              context.read<WishlistCubit>().addToWishlist(courseId);
            }
          },
          iconSize: size,
          icon: Icon(
            added ? Icons.favorite : Icons.favorite_border,
            color: Theme.of(context).primaryColor,
          ),
        );
      },
    );
  }
}
