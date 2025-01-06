import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

class EditThumbnailTab extends StatefulWidget {
  const EditThumbnailTab({super.key});

  @override
  State<EditThumbnailTab> createState() => _EditThumbnailTabState();
}

class _EditThumbnailTabState extends State<EditThumbnailTab>
    with AutomaticKeepAliveClientMixin<EditThumbnailTab> {
  @override
  bool get wantKeepAlive => true;

  @override
  Widget build(BuildContext context) {
    super.build(context);

    return const Text('THUMBNAIL');
  }
}
