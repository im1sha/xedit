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
using XEdit.TouchTracking;

namespace XEdit.Core
{
    public class ImageManager
    {
        private static readonly Lazy<ImageManager> _instance = new Lazy<ImageManager>(() => new ImageManager());

        public static ImageManager Instance { get => _instance.Value; }



        #region _backupImage

        private SKBitmap _backupImage;

        public void CreateBackupImage()
        {
            _backupImage = null;
            GC.Collect();
            _backupImage = CloneImage(_image);
        }

        public SKBitmap GetBackupImage()
        {
            return CloneImage(_backupImage);
        }

        #endregion



        #region _tempBitmap

        // for Transparency effect displaying
        private SKBitmap _tempBitmap;

        public SKBitmap TempBitmap
        {
            get
            {
                lock (this)
                {
                    return _tempBitmap;
                }
            }
            set
            {
                lock (this)
                {
                    // fixes memory usage increasing when filter 's applying continiously  
                    _tempBitmap = null;
                    GC.Collect();

                    _tempBitmap = value;
                }
            }
        }

        #endregion



        #region _image

        public static SKBitmap _image;

        public void SetImage(SKBitmap value)
        {
            lock (this)
            {
                _image = null;
                GC.Collect();
                _image = value;
            }
        }

        /// <summary>
        /// Clones active image
        /// </summary>
        /// <returns></returns>
        public SKBitmap CloneImage()
        {
            return CloneImage(_image);
        }

        private SKBitmap CloneImage(SKBitmap target)
        {
            if (target == null)
            {
                return null;
            }
            SKBitmap result = new SKBitmap(target.Info);
            lock (target)
            {
                target.CopyTo(result);
            } 
            return result;
        }

        public SKImageInfo GetImageInfo()
        {
            lock (this)
            {
                return _image.Info;
            }
        }

        public async Task<bool> Save()
        {
            bool success = false;
            SKBitmap bitmap = AppDispatcher.Get<ImageManager>().CloneImage(); 

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

            bitmap = null;
            GC.Collect();

            return success;
        }

        #endregion



        #region _canvasView

        public bool IsCanvasViewInitialized => _canvasView != null;

        private SKCanvasView _canvasView;

        public float ViewCanvasSizeWidth => _canvasView.CanvasSize.Width;
        public double ViewWidth => _canvasView.Width;
        public float ViewCanvasSizeHeight => _canvasView.CanvasSize.Height;
        public double ViewHeight => _canvasView.Height;

        public void SetCanvasViewReference(SKCanvasView c)
        {
            _canvasView = c;
            _canvasView.PaintSurface += _standardCanvasUpdateHandler;
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

        public void InvalidateCanvasView()
        {
            if (IsCanvasViewInitialized)
            {
                _canvasView.InvalidateSurface();
            }
        }

        #endregion



        #region _variableValuesSlider

        public bool IsSliderInitialized => _variableValuesSlider != null;

        private Slider _variableValuesSlider;

        public double SliderValue {
            get { lock (this) { return _variableValuesSlider.Value; } }
            set
            {
                lock (this)
                {
                    if ((value <= _variableValuesSlider.Maximum) && (value >= _variableValuesSlider.Minimum))
                    {
                        _variableValuesSlider.Value = value;
                        return;
                    }
                    throw new ApplicationException("Invalid value has passed to SliderValue");
                }             
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
            _variableValuesSlider.ValueChanged += _standardSliderUpdateHandler;
        }

        #endregion



        #region _touchEffect

        public bool IsTouchEffectInitialized => _touchEffect != null;

        private TouchEffect _touchEffect;

        private EventHandler<TouchActionEventArgs> _previousTouchEffectUpdateHandler =
            _standardTouchEffectUpdateHandler;
        private readonly static EventHandler<TouchActionEventArgs> _standardTouchEffectUpdateHandler =
            (sender, args) => { };

        public void SetTouchEffectUpdateHandler(EventHandler<TouchActionEventArgs> eh = null)
        {
            if (eh == null)
            {
                _touchEffect.Capture = false;
                eh = _standardTouchEffectUpdateHandler;
            }
            else
            {
                _touchEffect.Capture = true;
            }
            
            _touchEffect.TouchAction -= _previousTouchEffectUpdateHandler;
            _previousTouchEffectUpdateHandler = eh;
            _touchEffect.TouchAction += eh;
        }

        public void SetTouchEffectReference(TouchEffect effect)
        {
            _touchEffect = effect;
            _touchEffect.Capture = false;
            _touchEffect.TouchAction += _standardTouchEffectUpdateHandler;
        }

        #endregion
    }
}


