﻿using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IMediaService<T>
    {
        Task<T> PickPhotoAsync();

        Task<T> PickVideoAsync();

        //event ImageChangedEventHandler ImageChangedEventHandler;
        //void ImageChanged(List<Media> images);
        Task<T> TakePhotoAsync();
        Task<T> TakeVideoAsync();
    }

    //public delegate void ImageChangedEventHandler(object sender, ImageChangedArgs e);

    //public class ImageChangedArgs : EventArgs
    //{
    //    public List<Media> Images { get; set; }
    //}
}