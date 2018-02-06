using System;
using System.Collections.Generic;
using Abstractions.Models;

namespace Abstractions.Services.Contracts
{
    public interface IMediaService
    {
        event ImageChangedEventHandler ImageChangedEventHandler;
        void ImageChanged(List<Media> images);
        void OpenGallery();
        void OpenCamera();
        void PickAsync();
    }

    public delegate void ImageChangedEventHandler(object sender, ImageChangedArgs e);

    public class ImageChangedArgs : EventArgs
    {
        public List<Media> Images { get; set; }
    }
}