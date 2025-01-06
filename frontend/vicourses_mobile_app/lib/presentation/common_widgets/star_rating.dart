import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';

class StarRating extends StatelessWidget {
  final double initialRating;
  final bool allowHalfRating;
  final bool readonly;
  final double itemSize;
  final EdgeInsetsGeometry itemPadding;
  final void Function(double)? onRatingUpdate;

  const StarRating({
    super.key,
    required this.initialRating,
    this.allowHalfRating = true,
    this.readonly = false,
    this.itemSize = 20,
    this.itemPadding = EdgeInsets.zero,
    this.onRatingUpdate,
  });

  @override
  Widget build(BuildContext context) {
    return RatingBar(
      ignoreGestures: readonly,
      initialRating: initialRating,
      itemSize: itemSize,
      direction: Axis.horizontal,
      allowHalfRating: allowHalfRating,
      itemCount: 5,
      ratingWidget: RatingWidget(
        full: const Icon(
          Icons.star_purple500_outlined,
          color: Colors.orangeAccent,
        ),
        half: const Icon(
          Icons.star_half,
          color: Colors.orangeAccent,
        ),
        empty: const Icon(
          Icons.star_border_outlined,
          color: Colors.orangeAccent,
        ),
      ),
      itemPadding: itemPadding,
      onRatingUpdate: onRatingUpdate ?? (_) {},
    );
  }
}
