namespace CourseService.Domain.Objects
{
    public class VideoFile
    {
        public string FileId { get; private set; }
        public string Url { get; private set; }
        public string FileName { get; private set; }
        public string? StreamFileUrl { get; private set; }
        public int Length { get; private set; }

        private VideoFile(string fileId, string url, string fileName)
        {
            FileId = fileId;
            Url = url;
            FileName = fileName;
        }

        public static VideoFile Create(string fileId, string url, string fileName, string? streamFileUrl = null, int length = 0)
        {
            return new VideoFile(fileId, url, fileName)
            {
                StreamFileUrl = streamFileUrl,
                Length = length
            };
        }
    }
}
