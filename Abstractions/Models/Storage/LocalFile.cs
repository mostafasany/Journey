namespace Abstractions.Models.Storage
{
    public class LocalFile
    {
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public double Size { get; set; }
        public string Path { get; set; }
    }
}