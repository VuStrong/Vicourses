class UploadResponse {
  final String token;
  final String url;
  final String fileId;
  final String originalFileName;

  UploadResponse({
    required this.token,
    required this.url,
    required this.fileId,
    required this.originalFileName,
  });

  static UploadResponse fromMap(Map<String, dynamic> data) {
    return UploadResponse(
      token: data['token'],
      url: data['url'],
      fileId: data['fileId'],
      originalFileName: data['originalFileName'],
    );
  }
}
