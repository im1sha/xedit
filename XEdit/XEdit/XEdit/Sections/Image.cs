using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using XEdit.Extensions;
using XEdit.TouchTracking;
using XEdit.Utils;
using XEdit.ViewModels;

namespace XEdit.Sections
{
    class Image : BaseSection
    {
        List<TouchManipulationBitmap> _bitmapCollection;
        Dictionary<long, TouchManipulationBitmap> _bitmapDictionary;

        private readonly ResourceLoader _resourceLoader;

        private readonly MainViewModel _mainVM;

        private SKBitmap _backgroundBitmap;

        public override string Name => "Image";

        public override Command SelectCommand
        {
            get => new Command(() => {
                _bitmapCollection = new List<TouchManipulationBitmap>();
                _bitmapDictionary = new Dictionary<long, TouchManipulationBitmap>();
            });
        }

        public override Command LeaveCommand
        {
            get
            {
                return new Command(() =>
                {
                    _mainVM.TouchWorker.SetUpdateHandler();
                    _mainVM.CanvasViewWorker.SetUpdateHandler();
                    SaveImage();
                });
            }
        }

        public Image(MainViewModel vm)
        {
            _mainVM = vm;
            _resourceLoader = _mainVM.ResourceLoader;

            Handlers = new ObservableCollection<VisualHandler>();

            var totalImages = _resourceLoader.GetAmountOfImages(ResourceLoader.ImageFolder.Image);

            for (int i = 0; i < totalImages; i++)
            {
                Handlers.Add(CreateHandler(i));             
            }        
        }

        private VisualHandler CreateHandler(int i)
        {
            string name = $"Image {i + 1}";

            return new VisualHandler(
                name: name,
                url: null,
                perform: () =>
                {             
                    _backgroundBitmap = _mainVM.ImageWorker.Image;
                    SKPoint position = new SKPoint();
                    _bitmapCollection.Add(new TouchManipulationBitmap(_resourceLoader.LoadSKBitmap(ResourceLoader.ImageFolder.Image, i))
                    {
                        Matrix = SKMatrix.MakeTranslation(position.X, position.Y),
                    });

                    _mainVM.TouchWorker.SetUpdateHandler(OnTouchEffectAction);
                    _mainVM.CanvasViewWorker.SetUpdateHandler(OnCanvasViewPaintSurface);
                    _mainVM.CanvasViewWorker.Invalidate();

                    _selectedHandler = null;
                },
                close: (success) => {
                 
                }
                );
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            // Convert Xamarin.Forms point to pixels
            Point pt = args.Location;
            SKPoint point =
                new SKPoint((float)(_mainVM.CanvasViewWorker.ViewCanvasSizeWidth * pt.X / _mainVM.CanvasViewWorker.ViewWidth),
                            (float)(_mainVM.CanvasViewWorker.ViewCanvasSizeHeight * pt.Y / _mainVM.CanvasViewWorker.ViewHeight));

            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    for (int i = _bitmapCollection.Count - 1; i >= 0; i--)
                    {
                        TouchManipulationBitmap bitmap = _bitmapCollection[i];

                        if (bitmap.HitTest(point))
                        {
                            // Move bitmap to end of collection
                            _bitmapCollection.Remove(bitmap);
                            _bitmapCollection.Add(bitmap);

                            // Do the touch processing
                            _bitmapDictionary.Add(args.Id, bitmap);
                            bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                            _mainVM.CanvasViewWorker.Invalidate();
                            break;
                        }
                    }
                    break;

                case TouchActionType.Moved:
                    if (_bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = _bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        _mainVM.CanvasViewWorker.Invalidate();
                    }
                    break;

                case TouchActionType.Released:
                case TouchActionType.Cancelled:
                    if (_bitmapDictionary.ContainsKey(args.Id))
                    {
                        TouchManipulationBitmap bitmap = _bitmapDictionary[args.Id];
                        bitmap.ProcessTouchEvent(args.Id, args.Type, point);
                        _bitmapDictionary.Remove(args.Id);
                        _mainVM.CanvasViewWorker.Invalidate();
                    }
                    break;
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(_mainVM.ImageWorker.Image, info.Rect, BitmapStretch.Uniform);     

            foreach (TouchManipulationBitmap tmbitmap in _bitmapCollection)
            {
                tmbitmap.Paint(canvas);
            }       
        }

        void SaveImage()
        {
            SKSize canvasViewSize = _mainVM.CanvasViewWorker.CanvasView.CanvasSize;
          
            _mainVM.ImageWorker.AddImageState();
            SKBitmap newBitmap = new SKBitmap(_backgroundBitmap.Info);
            _backgroundBitmap.CopyTo(newBitmap);

            using (SKCanvas canvas = new SKCanvas(newBitmap))
            using (var paint = new SKPaint())
            {
                foreach (TouchManipulationBitmap tmBitmap in _bitmapCollection)
                {
                    var matrix = tmBitmap.Matrix;
                    var bitmap = tmBitmap.Bitmap;
                    float scale;
                    SKRect backgroundImageRect; 
                    (scale, backgroundImageRect) = SizeCalculator.GetScaleAndRect(canvasViewSize, _backgroundBitmap);

                    SKMatrix shiftMatrix = new SKMatrix();
                    SKPoint point = new SKPoint(-backgroundImageRect.Left * scale, -backgroundImageRect.Top * scale);
                    shiftMatrix.SetScaleTranslate(scale, scale, point.X, point.Y);
                    
                    canvas.Save();
                    canvas.Concat(ref shiftMatrix);
                    canvas.Concat(ref matrix);
                    canvas.DrawBitmap(tmBitmap.Bitmap, 0, 0);
                    canvas.Restore();
                }
            }
            _mainVM.ImageWorker.Image = newBitmap;
            _mainVM.CanvasViewWorker.Invalidate();
        }


        /// <summary>
        /// Converts position of point on canva to position on image
        /// </summary>
        private SKPoint ConvertToPositionOnImage(SKPoint point, SKRect rect,
            float bitmapSizeToRectSize)
        {
            return new SKPoint(((float)point.X - rect.Left) * bitmapSizeToRectSize,
                ((float)point.Y - rect.Top) * bitmapSizeToRectSize);
        }


        private SKPoint ConvertToPixel(Point point)
        {
            return new SKPoint(
                (float)(_mainVM.CanvasViewWorker.ViewCanvasSizeWidth * point.X /
                _mainVM.CanvasViewWorker.ViewWidth),
                (float)(_mainVM.CanvasViewWorker.ViewCanvasSizeHeight * point.Y /
                _mainVM.CanvasViewWorker.ViewHeight)
                );
        }
    }
}
