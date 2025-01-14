import 'package:chewie/chewie.dart';
import 'package:flutter/material.dart';
import 'package:vicourses_mobile_app/services/api/storage_service.dart';
import 'package:video_player/video_player.dart';

class HlsVideoPlayer extends StatefulWidget {
  final String token;

  const HlsVideoPlayer({
    super.key,
    required this.token,
  });

  @override
  State<HlsVideoPlayer> createState() => _HlsVideoPlayerState();
}

class _HlsVideoPlayerState extends State<HlsVideoPlayer> {
  late StorageService _storageService;
  late VideoPlayerController _videoPlayerController;
  ChewieController? _chewieController;

  Future<void> _initializePlayer() async {
    _storageService = StorageService();

    final response = await _storageService.getHlsManifest(widget.token);
    if (response == null) return;

    final map = _parseQueryParameters(response.params);

    final cookies =
        'CloudFront-Key-Pair-Id=${map['Key-Pair-Id']}; CloudFront-Policy=${map['Policy']}; CloudFront-Signature=${map['Signature']}';

    _videoPlayerController = VideoPlayerController.networkUrl(
      Uri.parse(response.manifestFileUrl),
      httpHeaders: {
        'Cookie': cookies,
      },
    );

    await _videoPlayerController.initialize();

    _chewieController = ChewieController(
      videoPlayerController: _videoPlayerController,
      aspectRatio: 16 / 9,
    );

    setState(() {});
  }

  Map<String, String> _parseQueryParameters(String query) {
    final Map<String, String> result = {};

    for (var pair in query.split('&')) {
      final parts = pair.split('=');
      if (parts.length == 2) {
        result[Uri.decodeComponent(parts[0])] = Uri.decodeComponent(parts[1]);
      }
    }

    return result;
  }

  @override
  void initState() {
    super.initState();
    _initializePlayer();
  }

  @override
  void dispose() {
    _videoPlayerController.dispose();
    _chewieController?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return AspectRatio(
      aspectRatio: 16 / 9,
      child: Center(
        child: _chewieController != null &&
            _chewieController!.videoPlayerController.value.isInitialized
            ? Chewie(controller: _chewieController!)
            : const CircularProgressIndicator(),
      ),
    );
  }
}
