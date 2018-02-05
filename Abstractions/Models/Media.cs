namespace Abstractions.Models
{
    public class Media
    {
        public string Id { get; set; }

        public byte[] SourceArray { get; set; }

        public string Path { get; set; }

        public string Thumbnail { get; set; }

        public string Ext { get; set; }

        public MediaType Type { get; set; }
    }

    public enum MediaType
    {
        Image,
        Video
    }
}