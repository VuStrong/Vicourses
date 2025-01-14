import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:vicourses_mobile_app/models/course.dart';
import 'package:vicourses_mobile_app/presentation/common_widgets/hls_video_player.dart';
import 'package:vicourses_mobile_app/utils/app_constants.dart';

class PreviewVideoScreen extends StatefulWidget {
  final CourseDetail course;

  const PreviewVideoScreen({
    super.key,
    required this.course,
  });

  @override
  State<PreviewVideoScreen> createState() => _PreviewVideoScreenState();
}

class _PreviewVideoScreenState extends State<PreviewVideoScreen> {
  @override
  Widget build(BuildContext context) {
    final course = widget.course;
    final previewVideo = course.previewVideo;

    return Scaffold(
      backgroundColor: Colors.black54,
      appBar: AppBar(
        title: Text(course.title),
        backgroundColor: Colors.black54,
        foregroundColor: Colors.white,
      ),
      body: Center(
        child:
            previewVideo != null && previewVideo.status == VideoStatus.processed
                ? HlsVideoPlayer(token: previewVideo.token)
                : Text(
                    AppLocalizations.of(context)!.noResults,
                    style: const TextStyle(color: Colors.white),
                    textAlign: TextAlign.center,
                  ),
      ),
    );
  }
}
