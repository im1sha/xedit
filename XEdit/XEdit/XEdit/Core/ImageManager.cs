using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

//void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
//{
//SKImageInfo info = args.Info;
//SKSurface surface = args.Surface;
//SKCanvas canvas = surface.Canvas;

//canvas.Clear();

//if (libraryBitmap != null)
//{
//    using (SKPaint paint = new SKPaint())
//    {
//        canvas.DrawBitmap(libraryBitmap, info.Rect, BitmapStretch.Uniform, paint: paint);
//    }
//}
//}

    // x:Name="skiaWrapper" 

namespace XEdit.Core
{
    public class ImageManager : INotifyPropertyChanged
    {
        private static readonly Lazy<ImageManager> instance = new Lazy<ImageManager>(() => new ImageManager());

        public static ImageManager Instance { get => instance.Value; }

        protected ImageManager() { }

        private SKBitmap _image;
        public SKBitmap Image
        {
            get
            {
                lock (this)
                {
                    return _image;
                }
            }
            set
            {
                lock (this)
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }
        }

        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        //public static bool IsImageLoaded { get { return ResourceBitmap != null; } }

        //public static SKBitmap ResourceBitmap { get; private set; }

        //public static void SetBitmap(SKBitmap b)
        //{
        //    if (b != null)
        //    {
        //        ResourceBitmap = b;
        //    }
        //}

        //public static bool ContainsSkCanvasView(object target)
        //{
        //    return IsTargetSuitable(target)
        //        && ((target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children.Count > 0)
        //        && ((target as Xamarin.Forms.Layout<Xamarin.Forms.View>).Children[0].GetType() == typeof(SKCanvasView));
        //}

        //public static bool IsTargetSuitable(object target)
        //{
        //    return target is Xamarin.Forms.Layout<Xamarin.Forms.View>;
        //}

        //public static bool AddNewCanvaAsChild(object target, SKCanvasView canvasView)
        //{
        //    if (IsTargetSuitable(target))
        //    {
        //        Xamarin.Forms.Layout<Xamarin.Forms.View> view = (target as Xamarin.Forms.Layout<Xamarin.Forms.View>);

        //        for (int i = 0; i < view.Children.Count; i++)
        //        {
        //            view.Children.RemoveAt(i);
        //        }
        //        view.BackgroundColor = Color.Black;

        //        view.Children.Add(canvasView);
        //        return true;
        //    }
        //    return false;
        //}

    }
}
