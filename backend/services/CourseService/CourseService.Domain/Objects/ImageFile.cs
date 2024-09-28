namespace CourseService.Domain.Objects
{
    public class ImageFile
    {
        public string FileId { get; private set; }
        public string Url { get; private set; }

        private ImageFile(string fileId, string url)
        {
            FileId = fileId;
            Url = url;
        }

        public static ImageFile Create(string fileId, string url)
        {
            return new ImageFile(fileId, url);
        }
    }
}
