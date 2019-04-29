using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XEdit.Extensions;

namespace XEdit.Core
{
    public class ImageManager : INotifyPropertyChanged
    {
        private static readonly Lazy<ImageManager> instance = new Lazy<ImageManager>(() => new ImageManager());

        public static ImageManager Instance { get => instance.Value; }

        protected ImageManager()
        {          
        }

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

        //SKCanvasView is not in use now. Image 's using instead
        public async Task<bool> Save()
        {
            bool success = false;
            SKBitmap bitmap = AppDispatcher.Get<ImageManager>().Image; // to CHANGE

            if (bitmap == null)
            {
                return false;
            }

            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg;
            int quality = 100;

            using (MemoryStream memStream = new MemoryStream())
            using (SKManagedWStream wstream = new SKManagedWStream(memStream))
            {
                bitmap.Encode(wstream, imageFormat, quality);
                byte[] data = memStream.ToArray();
                if (data != null && data.Length != 0)
                {
                    bool isGranted = DependencyService.Get<IUtils>().AskForWriteStoragePermission();
                    if (isGranted)
                    {
                        success = await DependencyService.Get<IPhotoLibrary>().
                            SavePhotoAsync(data, "testFolder", DateTime.Now.ToBinary().ToString() + ".png");
                    }
                }
            }
            return success;
        }

        #region _canvasView

        private SKCanvasView _canvasView;

        public void SetCanvasViewReference(SKCanvasView cw)
        {
            // IT'LL BE THROWN WHEN NAVIGATING BACK AND SELECT PICTURE AGAIN 
            //if (_canvasView != null)
            //{
            //    throw new ApplicationException("_canvasView is already set");
            //}

            _canvasView = cw;

            //initial update handler
            _canvasView.PaintSurface += _previousUpdateHandler;
        }

        private EventHandler<SKPaintSurfaceEventArgs> _previousUpdateHandler = (sender, args) =>
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.DrawBitmap(AppDispatcher.Get<ImageManager>().Image, info.Rect, BitmapStretch.Uniform);
        };

        public void SetUpdateHandler(EventHandler<SKPaintSurfaceEventArgs> eh)
        {
            _canvasView.PaintSurface -= _previousUpdateHandler;
            _previousUpdateHandler = eh;
            _canvasView.PaintSurface += eh;
        }

        #endregion

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
