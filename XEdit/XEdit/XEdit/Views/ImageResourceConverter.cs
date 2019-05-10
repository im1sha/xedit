using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEdit.Views
{
    /// <summary>
    /// Loads images using a Resource ID specified in XAML
    /// </summary>
    //[ContentProperty(nameof(Source))]
    //public class ImageResourceExtension : IMarkupExtension
    //{
    //    public string Source { get; set; }

    //    public object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        if (Source == null)
    //        {
    //            return null;
    //        }

    //        var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

    //        return imageSource;
    //    }
    //}

    public class ImageResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageSource = ImageSource.FromResource((string)value, typeof(ImageResourceConverter).GetTypeInfo().Assembly);
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
