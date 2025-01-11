import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/rating.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/star_rating.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class RatingItem extends StatelessWidget {
  final Rating rating;
  final Widget? action;

  const RatingItem({
    super.key,
    required this.rating,
    this.action,
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        ListTile(
          contentPadding: EdgeInsets.zero,
          leading: SizedBox(
            width: 40,
            height: 40,
            child: ClipOval(
              child: rating.user.thumbnailUrl != null
                  ? Image.network(
                      rating.user.thumbnailUrl!,
                      fit: BoxFit.cover,
                    )
                  : Image.asset(
                      AppConstants.defaultUserImagePath,
                      fit: BoxFit.cover,
                    ),
            ),
          ),
          title: Text(
            rating.user.name,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 16,
            ),
          ),
          subtitle: Row(
            children: [
              StarRating(
                initialRating: rating.star.toDouble(),
                readonly: true,
                itemSize: 18,
              ),
              const SizedBox(width: 5),
              Expanded(
                child: Text(
                  '${rating.createdAt.day}/${rating.createdAt.month}/${rating.createdAt.year}',
                  overflow: TextOverflow.ellipsis,
                  style: const TextStyle(
                    fontSize: 12,
                  ),
                ),
              ),
            ],
          ),
          trailing: action,
        ),
        Text(rating.feedback),
        if (rating.responded) _instructorResponse(context),
      ],
    );
  }

  Widget _instructorResponse(BuildContext context) {
    final respondedAt =
        '${rating.respondedAt!.day}/${rating.respondedAt!.month}/${rating.respondedAt!.year}';

    return Padding(
      padding: const EdgeInsets.only(left: 20, top: 15),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            '${AppLocalizations.of(context)!.instructorResponse} - $respondedAt',
            style: const TextStyle(fontWeight: FontWeight.bold),
          ),
          Text(rating.response ?? ''),
        ],
      ),
    );
  }
}
