using ContactsLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace ContactApp.utils
{
    class ImageSourceConverter : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Photo photo = value as Photo;
            if (photo != null)
            {
                if(photo.ImagePath != null && File.Exists(photo.ImagePath))
                    return ImageSource.FromFile(photo.ImagePath);
            }
            return "users.png";
        }

       

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
