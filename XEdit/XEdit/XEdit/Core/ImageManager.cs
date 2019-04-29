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
        private static readonly Lazy<ImageManager> _instance = new Lazy<ImageManager>(() => new ImageManager());

        public static ImageManager Instance { get => _instance.Value; }

        protected ImageManager()
        {          
        }

        // for Transparency effect displaying
        private SKBitmap _tempBitmap;
        public SKBitmap TempBitmap
        {
            get => _tempBitmap;
            set
            {
                _tempBitmap = value;
                if (IsCanvasViewInitialized)
                {
                    UpdateCanvasView();
                }
            }
        }

        public static SKBitmap _image;

        public void SetImage(SKBitmap value)
        {
            _image = value;
            //OnPropertyChanged();
            if (IsCanvasViewInitialized)
            {
                UpdateCanvasView();
            }
        }

        // replace with cloning
        //return new SKBitmap(_image.Info);
        public SKBitmap GetImage()
        {
            return _image;
        }

        //SKCanvasView is not in use now. Image 's using instead
        public async Task<bool> Save()
        {
            bool success = false;
            SKBitmap bitmap = AppDispatcher.Get<ImageManager>().GetImage(); // to CHANGE

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
                            SavePhotoAsync(data, "testFolder", DateTime.Now.ToBinary().ToString() + ".jpeg");
                    }
                }
            }
            return success;
        }



        #region _canvasView

        private bool IsCanvasViewInitialized => _canvasView != null;

        private SKCanvasView _canvasView;   

        public void SetCanvasViewReference(SKCanvasView c)
        {
            // IT'LL BE THROWN WHEN NAVIGATING BACK AND SELECT PICTURE AGAIN 
            //if (_canvasView != null)
            //{
            //    throw new ApplicationException("_canvasView is already set");
            //}

            _canvasView = c;

            //initial update handler
            _canvasView.PaintSurface += _previousCanvasUpdateHandler;
        }

        private EventHandler<SKPaintSurfaceEventArgs> _previousCanvasUpdateHandler = _standardCanvasUpdateHandler;

        private readonly static EventHandler<SKPaintSurfaceEventArgs> _standardCanvasUpdateHandler = (sender, args) =>
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.DrawBitmap(_image, info.Rect, BitmapStretch.Uniform);
        };

        /// <summary>
        /// Sets rendering handler for SKCanvasView
        /// </summary>
        /// <param name="eh">If null passed then it should set standard handler</param>
        public void SetCanvasUpdateHandler(EventHandler<SKPaintSurfaceEventArgs> eh = null)
        {
            if (eh == null)
            {
                eh = _standardCanvasUpdateHandler;
            }
            _canvasView.PaintSurface -= _previousCanvasUpdateHandler;
            _previousCanvasUpdateHandler = eh;
            _canvasView.PaintSurface += eh;
        }

        private void UpdateCanvasView()
        {
            _canvasView.InvalidateSurface();
        }

        #endregion



        #region _variableValuesSlider

        private bool IsSliderInitialized => _variableValuesSlider != null;

        private Slider _variableValuesSlider;

        public double SliderValue {
            get => _variableValuesSlider.Value;
            set
            {
                if ((value <= _variableValuesSlider.Maximum) && (value >= _variableValuesSlider.Minimum))
                {
                    _variableValuesSlider.Value = value;
                    return;
                }
                throw new ApplicationException("Invalid value has passed to SliderValue");
            }
        }

        private EventHandler<ValueChangedEventArgs> _previousSliderUpdateHandler = _standardSliderUpdateHandler;
        private readonly static EventHandler<ValueChangedEventArgs> _standardSliderUpdateHandler = (sender, args) => { };

        public void SetSliderUpdateHandler(EventHandler<ValueChangedEventArgs> eh = null)
        {
            if (eh == null)
            {
                eh = _standardSliderUpdateHandler;
            }
            _variableValuesSlider.ValueChanged -= _previousSliderUpdateHandler;
            _previousSliderUpdateHandler = eh;
            _variableValuesSlider.ValueChanged += eh;
        }

        public void SetSliderReference(Slider s)
        {
            _variableValuesSlider = s;
            _variableValuesSlider.ValueChanged += _previousSliderUpdateHandler;
        }

        #endregion



        #region INotifyPropertyChanged Support

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
     
    }
}
