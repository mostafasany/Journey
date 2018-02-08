using System;
using System.IO;
using Xamarin.Forms;

namespace Journey.Models
{
    public class Media
    {
        public string Id { get; set; }

        public byte[] SourceArray { get; set; }

        public string Path { get; set; }

        public string Thumbnail { get; set; }

        public string Ext { get; set; }

        public MediaType Type { get; set; }

        public ImageSource Source
        {
            get
            {
                ImageSource temp = null;
                if (Path != null && Path.ToLower().Contains("http"))
                {
                    temp = ImageSource.FromUri(new Uri(Path));
                }
                else
                {
                    if (SourceArray != null)
                    {
                        var stream = new MemoryStream(SourceArray);
                        temp = ImageSource.FromStream(() => stream);
                    }
                    //else if(TempSource!=null)
                    //{
                    //  return TempSource;
                    //}
                }
                return temp;
            }
        }
    }

    public enum MediaType
    {
        Image,
        Video
    }
}