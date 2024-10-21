using CourseService.Domain.Enums;

namespace CourseService.Domain.Objects
{
    public class VideoFile
    {
        public string FileId { get; private set; }
        public string Url { get; private set; }
        public string OriginalFileName { get; private set; }
        public string? StreamFileUrl { get; private set; }
        public int Duration { get; private set; }
        public VideoStatus Status { get; private set; }

        private VideoFile(string fileId, string url, string originalFileName)
        {
            FileId = fileId;
            Url = url;
            OriginalFileName = originalFileName;
            Status = VideoStatus.BeingProcessed;
        }

        public static VideoFile Create(string fileId, string url, string originalFileName, string? streamFileUrl = null, int duration = 0,
            VideoStatus status = VideoStatus.BeingProcessed)
        {
            return new VideoFile(fileId, url, originalFileName)
            {
                StreamFileUrl = streamFileUrl,
                Duration = duration,
                Status = status,
            };
        }
    }
}
