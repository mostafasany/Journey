﻿using System;
using Xamarin.Forms;

namespace Journey.Views.Controls
{
    public class ImageControl : Image
    {
        public static readonly BindableProperty PortablePathProperty =
            BindableProperty.Create(nameof(PortablePath), typeof(string), typeof(ImageControl), null,
                BindingMode.TwoWay,
                null,
                (bindable, oldValue, newValue) =>
                {
                    var val = newValue.ToString();
                    try
                    {
                        (bindable as ImageControl).Source = Device.RuntimePlatform == Device.UWP
                            ? ImageSource.FromFile(val.Replace("Journey.Assets.", ""))
                            : ImageSource.FromResource(val);
                    }
                    catch (Exception e)
                    {
                    }
                });

        public string PortablePath
        {
            get => (string) GetValue(PortablePathProperty);
            set => SetValue(PortablePathProperty, value);
        }
    }
}